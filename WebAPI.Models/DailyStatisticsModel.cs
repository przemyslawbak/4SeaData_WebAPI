using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class DailyStatisticsModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CargoMoving { get; set; }
        public int CargoNotMoving { get; set; }
        public int CargoMissing { get; set; }
        public int CargoExpired { get; set; }
        public int DredgingMoving { get; set; }
        public int DredgingNotMoving { get; set; }
        public int DredgingMissing { get; set; }
        public int DredgingExpired { get; set; }
        public int FishingMoving { get; set; }
        public int FishingNotMoving { get; set; }
        public int FishingMissing { get; set; }
        public int FishingExpired { get; set; }
        public int OffshoreMoving { get; set; }
        public int OffshoreNotMoving { get; set; }
        public int OffshoreMissing { get; set; }
        public int OffshoreExpired { get; set; }
        public int OtherMoving { get; set; }
        public int OtherNotMoving { get; set; }
        public int OtherMissing { get; set; }
        public int OtherExpired { get; set; }
        public int PassengerMoving { get; set; }
        public int PassengerNotMoving { get; set; }
        public int PassengerMissing { get; set; }
        public int PassengerExpired { get; set; }
        public int TankerMoving { get; set; }
        public int TankerNotMoving { get; set; }
        public int TankerMissing { get; set; }
        public int TankerExpired { get; set; }
        public int TugMoving { get; set; }
        public int TugNotMoving { get; set; }
        public int TugMissing { get; set; }
        public int TugExpired { get; set; }
    }
}
