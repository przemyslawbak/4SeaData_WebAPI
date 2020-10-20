using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<string> areaNames = GetSeaAreas().Select(a => a.Name).ToList();
            areaNames.Add("All areas");
            areaNames.Add("");

            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFStatRepository _repo = scope.ServiceProvider.GetRequiredService<EFStatRepository>();

                foreach (string areaName in areaNames)
                {
                    DailyStatisticsModel updateStats = new DailyStatisticsModel()
                    {
                        Id = 0,
                        Date = DateTime.UtcNow,
                        CargoMoving = _repo.GetCargoMoving(areaName),
                        CargoNotMoving = _repo.GetCargoNotMoving(areaName),
                        CargoAnchored = _repo.GetCargoAnchored(areaName),
                        DredgingMoving = _repo.GetDredgingMoving(areaName),
                        DredgingNotMoving = _repo.GetDredgingNotMoving(areaName),
                        DredgingAnchored = _repo.GetDredgingAnchored(areaName),
                        FishingMoving = _repo.GetFishingMoving(areaName),
                        FishingNotMoving = _repo.GetFishingNotMoving(areaName),
                        FishingAnchored = _repo.GetFishingAnchored(areaName),
                        OffshoreMoving = _repo.GetOffshoreMoving(areaName),
                        OffshoreNotMoving = _repo.GetOffshoreNotMoving(areaName),
                        OffshoreAnchored = _repo.GetOffshoreAnchored(areaName),
                        OtherMoving = _repo.GetOtherMoving(areaName),
                        OtherNotMoving = _repo.GetOtherNotMoving(areaName),
                        OtherAnchored = _repo.GetOtherAnchored(areaName),
                        PassengerMoving = _repo.GetPassengerMoving(areaName),
                        PassengerNotMoving = _repo.GetPassengerNotMoving(areaName),
                        PassengerAnchored = _repo.GetPassengerAnchored(areaName),
                        TankerMoving = _repo.GetTankerMoving(areaName),
                        TankerNotMoving = _repo.GetTankerNotMoving(areaName),
                        TankerAnchored = _repo.GetTankerAnchored(areaName),
                        TugMoving = _repo.GetTugMoving(areaName),
                        TugNotMoving = _repo.GetTugNotMoving(areaName),
                        TugAnchored = _repo.GetTugAnchored(areaName),
                        Area = areaName
                    };

                    _repo.SaveStatistics(updateStats);
                }
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
