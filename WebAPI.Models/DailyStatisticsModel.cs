using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class DailyStatisticsModel
    {
        [Key]
        public string Area { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CargoMoving { get; set; }
        public int CargoNotMoving { get; set; }
        public int CargoAnchored { get; set; }
        public int DredgingMoving { get; set; }
        public int DredgingNotMoving { get; set; }
        public int DredgingAnchored { get; set; }
        public int FishingMoving { get; set; }
        public int FishingNotMoving { get; set; }
        public int FishingAnchored { get; set; }
        public int OffshoreMoving { get; set; }
        public int OffshoreNotMoving { get; set; }
        public int OffshoreAnchored { get; set; }
        public int OtherMoving { get; set; }
        public int OtherNotMoving { get; set; }
        public int OtherAnchored { get; set; }
        public int PassengerMoving { get; set; }
        public int PassengerNotMoving { get; set; }
        public int PassengerAnchored { get; set; }
        public int TankerMoving { get; set; }
        public int TankerNotMoving { get; set; }
        public int TankerAnchored { get; set; }
        public int TugMoving { get; set; }
        public int TugNotMoving { get; set; }
        public int TugAnchored { get; set; }
    }
}
