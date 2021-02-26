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
        bool VerifyIfVesselArrivedPortAndNotDeparted(string currnetPortLocode, int iMO);
        void VesselDeparture(int iMO, System.DateTime? aISLatestActivity);
        void VesselArrival(string currnetPortLocode, int iMO, System.DateTime? aISLatestActivity);
        void UpdatePort(string currnetPortLocode, int iMO);
    }
}