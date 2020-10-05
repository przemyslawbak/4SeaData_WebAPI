﻿using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using WebAPI.DAL;
using WebAPI.Models;

namespace WebAPI.Services
{
    //todo: refactor
    //todo: Resilient connections - https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/work-with-data-in-asp-net-core-apps
    public class DataAccessService : IDataAccessService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DataAccessService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
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

        public void SaveUpdatedVessel(VesselUpdateModel updatedVessels) //todo: make sure if not null
        {
            using (IServiceScope scope = _scopeFactory.CreateScope())
            {
                IDataRepository _repo = scope.ServiceProvider.GetRequiredService<DataRepository>();
                //_repo.SaveUpdatedVessels(updatedVessels); todo: uncomment, check repo
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
    }
}
