using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using WebAPI.DAL;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISqlQueryBuilder _queryBuilder;
        private readonly IUpdatingProgress _progress;

        public DataAccessService(IServiceScopeFactory scopeFactory, ISqlQueryBuilder queryBuilder, IUpdatingProgress progress)
        {
            _scopeFactory = scopeFactory;
            _queryBuilder = queryBuilder;
            _progress = progress;
        }

        public List<SeaModel> GetSeaAreas()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFRepository _repo = scope.ServiceProvider.GetRequiredService<EFRepository>();
                return _repo.GetAllSeaAreas();
            };
        }

        public List<VesselAisUpdateModel> GetVesselsToBeUpdated()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFRepository _repo = scope.ServiceProvider.GetRequiredService<EFRepository>();
                return _repo.GetAllVesselsUpdateModels();
            };
        }

        public void SaveDatabaseQuantities()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFRepository _repo = scope.ServiceProvider.GetRequiredService<EFRepository>();
                _repo.SaveDatabaseQuantities();
            };
        }

        public void SaveUpdatedVessels(List<VesselUpdateModel> updatedVessels)
        {
            if (updatedVessels.Count > 0)
            {
                UpdateAllVessels(updatedVessels);
            }
        }

        public void SaveUpdateLogs(StatusModel statusModel)
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFRepository _repo = scope.ServiceProvider.GetRequiredService<EFRepository>();

                var updateLog = new UpdateLogModel()
                {
                    TimeCompleted = DateTime.UtcNow,
                    TimeStarted = statusModel.LastStartedTime,
                    MissingActivityTimes = statusModel.MissingActivityTimes,
                    MissingAreas = statusModel.MissingAreas,
                    FailedResultsQuantity = statusModel.FailedResultsQuantity,
                    MissingCourses = statusModel.MissingCourses,
                    MissingDestinations = statusModel.MissingDestinations,
                    MissingDraughts = statusModel.MissingDraughts,
                    MissingEtas = statusModel.MissingEtas,
                    MissingLats = statusModel.MissingLats,
                    MissingLongs = statusModel.MissingLongs,
                    MissingSpeeds = statusModel.MissingSpeeds,
                    MissingStatuses = statusModel.MissingStatuses,
                    ReurnedVesselsInCurrent = statusModel.ReurnedVesselsInCurrent,
                    TotalResultsQuantity = statusModel.TotalResultsQuantity,
                    SkippedResultsQuantity = statusModel.SkippedResultsQuantity,
                };

                _repo.SaveUpdateLogs(updateLog);
            };
        }

        private void UpdateAllVessels(List<VesselUpdateModel> updatedVessels)
        {
            if (updatedVessels.Count > 0)
            {
                try
                {
                    _queryBuilder.CreateAndSendUpdatesQuery(updatedVessels); //todo: unit test catch
                }
                catch (Exception ex)
                {
                    _progress.SetLastError(ex.Message);
                    _progress.AddFailedRequest();
                }
            }
        }
    }
}
