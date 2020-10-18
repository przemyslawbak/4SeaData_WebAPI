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

                UpdateLogModel updateLog = new UpdateLogModel()
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

        public void UpdateDailyStatistics()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFStatRepository _repo = scope.ServiceProvider.GetRequiredService<EFStatRepository>();

                DailyStatisticsModel updateStats = new DailyStatisticsModel()
                {
                    Date = DateTime.UtcNow,
                    CargoExpired = _repo.GetCargoExpired(),
                    CargoMissing = _repo.GetCargoMissing(),
                    CargoMoving = _repo.GetCargoMoving(),
                    CargoNotMoving = _repo.GetCargoNotMoving(),
                    CargoAnchored = _repo.GetCargoAnchored(),
                    DredgingExpired = _repo.GetDredgingExpired(),
                    DredgingMissing = _repo.GetDredgingMissing(),
                    DredgingMoving = _repo.GetDredgingMoving(),
                    DredgingNotMoving = _repo.GetDredgingNotMoving(),
                    DredgingAnchored = _repo.GetDredgingAnchored(),
                    FishingExpired = _repo.GetFishingExpired(),
                    FishingMissing = _repo.GetFishingMissing(),
                    FishingMoving = _repo.GetFishingMoving(),
                    FishingNotMoving = _repo.GetFishingNotMoving(),
                    FishingAnchored = _repo.GetFishingAnchored(),
                    OffshoreExpired = _repo.GetOffshoreExpired(),
                    OffshoreMissing = _repo.GetOffshoreMissing(),
                    OffshoreMoving = _repo.GetOffshoreMoving(),
                    OffshoreNotMoving = _repo.GetOffshoreNotMoving(),
                    OffshoreAnchored = _repo.GetOffshoreAnchored(),
                    OtherExpired = _repo.GetOtherExpired(),
                    OtherMissing = _repo.GetOtherMissing(),
                    OtherMoving = _repo.GetOtherMoving(),
                    OtherNotMoving = _repo.GetOtherNotMoving(),
                    OtherAnchored = _repo.GetOtherAnchored(),
                    PassengerExpired = _repo.GetPassengerExpired(),
                    PassengerMissing = _repo.GetPassengerMissing(),
                    PassengerMoving = _repo.GetPassengerMoving(),
                    PassengerNotMoving = _repo.GetPassengerNotMoving(),
                    PassengerAnchored = _repo.GetPassengerAnchored(),
                    TankerExpired = _repo.GetTankerExpired(),
                    TankerMissing = _repo.GetTankerMissing(),
                    TankerMoving = _repo.GetTankerMoving(),
                    TankerNotMoving = _repo.GetTankerNotMoving(),
                    TankerAnchored = _repo.GetTankerAnchored(),
                    TugExpired = _repo.GetTugExpired(),
                    TugMissing = _repo.GetTugMissing(),
                    TugMoving = _repo.GetTugMoving(),
                    TugNotMoving = _repo.GetTugNotMoving(),
                    TugAnchored = _repo.GetTugAnchored()
                };

                _repo.SaveStatistics(updateStats);
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
