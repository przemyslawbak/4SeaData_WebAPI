﻿using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdatedVesselFactory : IUpdatedVesselFactory
    {
        private readonly IScrapper _scrapper;
        private readonly IUpdatingProgress _progress;
        private readonly IExceptionProcessor _exceptionProcessor;

        public UpdatedVesselFactory(IScrapper scrapper, IUpdatingProgress progress, IExceptionProcessor exceptionProcessor)
        {
            _scrapper = scrapper;
            _progress = progress;
            _exceptionProcessor = exceptionProcessor;
        }

        public async Task<VesselUpdateModel> GetVesselUpdatesAsync(VesselAisUpdateModel aisUpdateModel)
        {
            VesselUpdateModel vessel = null;

            try
            {
                bool skip = false;

                if (aisUpdateModel.Mmsi == 0 || !aisUpdateModel.Speed.HasValue) //if could not be scrapped with "full"
                {
                    skip = true;
                    _progress.AddSkipped();
                }

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
            }
            catch (Exception ex)
            {
                _progress.SetLastError(ex.Message + " from: " + _exceptionProcessor.GetMethodNameThrowingException(ex));
                _progress.AddFailedRequest();
                vessel = null;
            }
            finally
            {
                while (_progress.GetIsUpdatingDatabase() || _progress.GetIsUpdatingPaused())
                {
                    await Task.Delay(100);
                }
            }

            return vessel;
        }
    }
}
