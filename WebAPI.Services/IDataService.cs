using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IDataAccessService
    {
        List<VesselAisUpdateModel> GetVesselsToBeUpdated();
        void UpdateDataQuantities();
        List<SeaModel> GetSeaAreas();
    }
}