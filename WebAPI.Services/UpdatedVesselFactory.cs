using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdatedVesselFactory : IVesselFactory
    {
        private readonly IScrapper _scrapper;
        private readonly IProgressService _progress;

        public UpdatedVesselFactory(IScrapper scrapper, IProgressService progress)
        {
            _scrapper = scrapper;
            _progress = progress;
        }

        public async Task<VesselUpdateModel> GetVesselUpdatesAsync(VesselAisUpdateModel aisUpdateModel, List<SeaModel> seaAreas, CancellationToken token, SemaphoreSlim semaphoreThrottel)
        {
            bool skip = false;
            VesselUpdateModel vessel = null;

            if (aisUpdateModel.Mmsi == 0 || !aisUpdateModel.Speed.HasValue) //if could not be scrapped with "full"
                skip = true;

            await semaphoreThrottel.WaitAsync();

            try
            {
                if (!skip)
                {
                    vessel = _scrapper.ScrapSingleVessel(aisUpdateModel.Mmsi, aisUpdateModel.Imo, seaAreas);

                    while (_progress.GetIsUpdatingDatabase() || _progress.GetIsUpdatingPaused())
                    {
                        await Task.Delay(100);
                    }
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
