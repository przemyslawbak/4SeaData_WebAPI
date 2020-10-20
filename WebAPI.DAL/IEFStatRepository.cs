using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IEFStatRepository
    {
        void SaveStatistics(DailyStatisticsModel updateStats);
        int GetCargoMoving(string areaName);
        int GetCargoNotMoving(string areaName);
        int GetCargoAnchored(string areaName);
        int GetDredgingMoving(string areaName);
        int GetDredgingNotMoving(string areaName);
        int GetDredgingAnchored(string areaName);
        int GetFishingMoving(string areaName);
        int GetFishingNotMoving(string areaName);
        int GetFishingAnchored(string areaName);
        int GetOffshoreMoving(string areaName);
        int GetOffshoreNotMoving(string areaName);
        int GetOffshoreAnchored(string areaName);
        int GetOtherMoving(string areaName);
        int GetOtherNotMoving(string areaName);
        int GetOtherAnchored(string areaName);
        int GetPassengerMoving(string areaName);
        int GetPassengerNotMoving(string areaName);
        int GetPassengerAnchored(string areaName);
        int GetTankerMoving(string areaName);
        int GetTankerNotMoving(string areaName);
        int GetTankerAnchored(string areaName);
        int GetTugMoving(string areaName);
        int GetTugNotMoving(string areaName);
        int GetTugAnchored(string areaName);
        int GetMoving(string areaName, string category);
        int GetMoored(string areaName, string category);
        int GetAnchored(string areaName, string category);
    }
}
