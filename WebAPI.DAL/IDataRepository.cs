using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IDataRepository
    {
        List<SeaModel> GetAllSeaAreas();
        List<VesselAisUpdateModel> GetAllVesselsUpdateModels();
        void SaveDatabaseQuantities();
    }
}