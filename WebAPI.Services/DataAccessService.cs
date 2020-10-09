﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using WebAPI.DAL;
using WebAPI.Models;

namespace WebAPI.Services
{
    //todo: Resilient connections - https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/work-with-data-in-asp-net-core-apps
    public class DataAccessService : IDataAccessService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISqlQueryBuilder _queryBuilder;

        public DataAccessService(IServiceScopeFactory scopeFactory, ISqlQueryBuilder queryBuilder)
        {
            _scopeFactory = scopeFactory;
            _queryBuilder = queryBuilder;
        }

        public List<SeaModel> GetSeaAreas()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IDataRepository _repo = scope.ServiceProvider.GetRequiredService<DataRepository>();
                return _repo.GetAllSeaAreas();
            };
        }

        public List<VesselAisUpdateModel> GetVesselsToBeUpdated()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IDataRepository _repo = scope.ServiceProvider.GetRequiredService<DataRepository>();
                return _repo.GetAllVesselsUpdateModels();
            };
        }

        public void SaveDatabaseQuantities()
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IDataRepository _repo = scope.ServiceProvider.GetRequiredService<DataRepository>();
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
            string query = string.Empty;

            if (updatedVessels.Count > 0)
            {
                query = _queryBuilder.GetCompleteQuery(updatedVessels);
            }
        }
    }
}
