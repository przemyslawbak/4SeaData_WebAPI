using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IEFStatRepository
    {
        void SaveStatistics(DailyStatisticsModel updateStats);
        int GetCargoExpired();
        int GetCargoMissing();
        int GetCargoMoving();
        int GetCargoNotMoving();
        int GetCargoAnchored();
        int GetDredgingExpired();
        int GetDredgingMissing();
        int GetDredgingMoving();
        int GetDredgingNotMoving();
        int GetDredgingAnchored();
        int GetFishingExpired();
        int GetFishingMissing();
        int GetFishingMoving();
        int GetFishingNotMoving();
        int GetFishingAnchored();
        int GetOffshoreExpired();
        int GetOffshoreMissing();
        int GetOffshoreMoving();
        int GetOffshoreNotMoving();
        int GetOffshoreAnchored();
        int GetOtherExpired();
        int GetOtherMissing();
        int GetOtherMoving();
        int GetOtherNotMoving();
        int GetOtherAnchored();
        int GetPassengerExpired();
        int GetPassengerMissing();
        int GetPassengerMoving();
        int GetPassengerNotMoving();
        int GetPassengerAnchored();
        int GetTankerExpired();
        int GetTankerMissing();
        int GetTankerMoving();
        int GetTankerNotMoving();
        int GetTankerAnchored();
        int GetTugExpired();
        int GetTugMissing();
        int GetTugMoving();
        int GetTugNotMoving();
        int GetTugAnchored();
    }
}
