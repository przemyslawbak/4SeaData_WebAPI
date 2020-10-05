using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdatedVesselFactory : IUpdatedVesselFactory
    {
        private readonly IScrapper _scrapper;
        private readonly IUpdatingProgress _progress;

        public UpdatedVesselFactory(IScrapper scrapper, IUpdatingProgress progress)
        {
            _scrapper = scrapper;
            _progress = progress;
        }

        public async Task<VesselUpdateModel> GetVesselUpdatesAsync(VesselAisUpdateModel aisUpdateModel, CancellationToken token, SemaphoreSlim semaphoreThrottel)
        {
            bool skip = false;
            VesselUpdateModel vessel = null;

            if (aisUpdateModel.Mmsi == 0 || !aisUpdateModel.Speed.HasValue) //if could not be scrapped with "full"
            {
                //skip = true;
                //_progress.AddSkipped();
            }

            await semaphoreThrottel.WaitAsync();

            try
            {
                if (!skip)
                {
                    vessel = _scrapper.ScrapSingleVessel(aisUpdateModel.Mmsi, aisUpdateModel.Imo);

                    if (vessel.IMO != aisUpdateModel.Imo)
                    {
                        throw new Exception("Received vessel imo differs from the one passed.");
                    }

                    while (_progress.GetIsUpdatingDatabase() || _progress.GetIsUpdatingPaused())
                    {
                        await Task.Delay(100);
                    }

                    _progress.AddToReturnedResultsQuantity();
                }
                else
                {
                    //todo: do nothing? or log?
                }
            }
            catch (Exception ex)
            {
                _progress.AddFailedRequest();
                _progress.SetLastError(ex.Message);
                vessel = null;
            }
            finally
            {
                while (_progress.GetIsUpdatingDatabase() || _progress.GetIsUpdatingPaused())
                {
                    await Task.Delay(100);
                }

                semaphoreThrottel.Release();
            }

            return vessel;
        }
    }
}
