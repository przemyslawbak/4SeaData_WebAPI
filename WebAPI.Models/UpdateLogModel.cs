using System;

namespace WebAPI.Models
{
    public class UpdateLogModel
    {
        public DateTime TimeStarted { get; set; }
        public DateTime TimeCompleted { get; set; }
        public int ReurnedVesselsInCurrent { get; set; }
        public int FailedResultsQuantity { get; set; }
        public int TotalResultsQuantity { get; set; }
        public int MissingLats { get; set; }
        public int MissingLongs { get; set; }
        public int MissingDestinations { get; set; }
        public int MissingDraughts { get; set; }
        public int MissingSpeeds { get; set; }
        public int MissingCourses { get; set; }
        public int MissingActivityTimes { get; set; }
        public int MissingEtas { get; set; }
        public int MissingStatuses { get; set; }
        public int MissingAreas { get; set; }
    }
}
