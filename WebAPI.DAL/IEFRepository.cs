using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IEFRepository
    {
        IQueryable<SeaModel> GetAllSeaAreas();
        List<VesselAisUpdateModel> GetAllVesselsUpdateModels();
        void SaveDatabaseQuantities();
        void SaveUpdateLogs(UpdateLogModel updateLog);
        IQueryable<PortModel> GetAllPorts();
    }
}