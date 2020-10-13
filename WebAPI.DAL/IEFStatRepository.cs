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
        int GetDredgingExpired();
        int GetDredgingMissing();
        int GetDredgingMoving();
        int GetDredgingNotMoving();
        int GetFishingExpired();
        int GetFishingMissing();
        int GetFishingMoving();
        int GetFishingNotMoving();
        int GetOffshoreExpired();
        int GetOffshoreMissing();
        int GetOffshoreMoving();
        int GetOffshoreNotMoving();
        int GetOtherExpired();
        int GetOtherMissing();
        int GetOtherMoving();
        int GetOtherNotMoving();
        int GetPassengerExpired();
        int GetPassengerMissing();
        int GetPassengerMoving();
        int GetPassengerNotMoving();
        int GetTankerExpired();
        int GetTankerMissing();
        int GetTankerMoving();
        int GetTankerNotMoving();
        int GetTugExpired();
        int GetTugMissing();
        int GetTugMoving();
        int GetTugNotMoving();
    }
}
