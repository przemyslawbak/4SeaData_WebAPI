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


        public IEnumerable<AreaBboxModel> GetPortAreas()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFRepository _repo = scope.ServiceProvider.GetRequiredService<EFRepository>();
                return from port in _repo.GetAllPorts()
                       select new AreaBboxModel
                       {
                           KeyProperty = port.PortLocode,
                           Name = port.NameAscii,
                           MaxLatitude = port.MaxLatitude,
                           MaxLongitude = port.MaxLongitude,
                           MinLatitude = port.MinLatitude,
                           MinLongitude = port.MinLongitude
                       };
            };
        }

        public IEnumerable<AreaBboxModel> GetSeaAreas()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFRepository _repo = scope.ServiceProvider.GetRequiredService<EFRepository>();
                return from port in _repo.GetAllSeaAreas()
                       select new AreaBboxModel
                       {
                           KeyProperty = port.MRGID.ToString(),
                           Name = port.Name,
                           MaxLatitude = port.MaxLatitude,
                           MaxLongitude = port.MaxLongitude,
                           MinLatitude = port.MinLatitude,
                           MinLongitude = port.MinLongitude
                       };
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

        //todo: unit tests
        public void UpdateDailyStatistics()
        {
            List<string> vesselCategories = new List<string>() { "Cargo", "Dredging", "Fishing", "Offshore", "Other", "Passenger", "Tanker", "Tug" };
            List<string> areaNames = GetSeaAreas().Select(a => a.Name).ToList();

            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFStatRepository _statsRepo = scope.ServiceProvider.GetRequiredService<EFStatRepository>();

                RemoveIncompleteStatsIfAny(_statsRepo);

                for (int i = 0; i < vesselCategories.Count(); i++)
                {
                    for (int j = 0; j < areaNames.Count(); j++)
                    {
                        DailyStatisticsModel updateStats = new DailyStatisticsModel()
                        {
                            Date = DateTime.UtcNow,
                            Moving = _statsRepo.GetMoving(areaNames[j], vesselCategories[i]),
                            Moored = _statsRepo.GetMoored(areaNames[j], vesselCategories[i]),
                            Anchored = _statsRepo.GetAnchored(areaNames[j], vesselCategories[i]),
                            VesselCategory = vesselCategories[i],
                            Area = areaNames[j]
                        };

                        _statsRepo.SaveStatistics(updateStats);
                    }
                }
            };
        }

        private void RemoveIncompleteStatsIfAny(IEFStatRepository _statsRepo)
        {
            List<DailyStatisticsModel> statsToBeRemoved = _statsRepo.GetAllStatsForToday();

            if (statsToBeRemoved.Count > 0)
            {
                _statsRepo.DeleteStats(statsToBeRemoved);
            }
        }

        public bool CheckIfStatsCompleteForToday()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IEFStatRepository _statsRepo = scope.ServiceProvider.GetRequiredService<EFStatRepository>();

                return _statsRepo.AreCompleteStatsForToday();
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
