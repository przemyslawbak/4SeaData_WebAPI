using System.Collections.Generic;
using WebAPI.DAL;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly IDataRepository _repo;

        public DataAccessService(IDataRepository repo)
        {
            _repo = repo;
        }

        public List<SeaModel> GetSeaAreas()
        {
            return _repo.GetAllSeaAreas();
        }

        public List<VesselAisUpdateModel> GetVesselsToBeUpdated()
        {
            return _repo.GetAllVesselsUpdateModels();
        }

        public void SaveUpdatedVessels(List<VesselModel> updatedVessels)
        {
            _repo.SaveUpdatedVessels(updatedVessels);
        }

        public void SaveDatabaseQuantities()
        {
            _repo.SaveDatabaseQuantities();
        }
    }
}
