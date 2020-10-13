using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IEFRepository
    {
        List<SeaModel> GetAllSeaAreas();
        List<VesselAisUpdateModel> GetAllVesselsUpdateModels();
        void SaveDatabaseQuantities();
        void SaveUpdateLogs(UpdateLogModel updateLog);
        void SaveStatistics(DailyStatisticsModel updateStats);
    }
}