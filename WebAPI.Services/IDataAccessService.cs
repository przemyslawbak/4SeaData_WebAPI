using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IDataAccessService
    {
        List<VesselAisUpdateModel> GetVesselsToBeUpdated();
        void SaveDatabaseQuantities();
        IEnumerable<PortModel> GetAllPorts();
        IEnumerable<SeaModel> GetSeaAreas();
        void SaveUpdatedVessels(List<VesselUpdateModel> updatedVessels);
        void SaveUpdateLogs(StatusModel statusModel);
        void UpdateDailyStatistics();
        bool CheckIfStatsCompleteForToday();
    }
}