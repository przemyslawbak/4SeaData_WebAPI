using System;

namespace WebAPI.Models
{
    public class StatusModel
    {
        public DateTime LastStartedTime { get; set; }
        public DateTime LastCompletedTime { get; set; }
        public DateTime NextPlannedTime { get; set; }
        public bool IsUpdatingPaused { get; set; }
        public bool IsUpdatingInProgress { get; set; }
        public bool Finalizing { get; set; }
        public bool UpdatingDatabase { get; set; }
        public int SkippedResultsQuantity { get; set; }
        public int FailedResultsQuantity { get; set; }
        public int TotalResultsQuantity { get; set; }
        public int ReurnedVesselsInCurrent { get; set; }
        public string LastUpdatedVessel { get; set; }
        public string LastError { get; set; }
        public float MemoryMegabytesUsage { get; set; }
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
