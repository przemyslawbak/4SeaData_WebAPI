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
