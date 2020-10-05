using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IDataAccessService
    {
        List<VesselAisUpdateModel> GetVesselsToBeUpdated();
        void SaveDatabaseQuantities();
        List<SeaModel> GetSeaAreas();
        void SaveUpdatedVessel(VesselUpdateModel updatedVessels);
    }
}