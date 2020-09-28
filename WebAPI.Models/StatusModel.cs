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
        public int FailedResultsQuantity { get; set; }
        public int TotalResultsQuantity { get; set; }
        public int ReurnedVesselsInCurrent { get; set; }
        public string LastUpdatedVessel { get; set; }
        public string LastError { get; set; }
        public float MemoryMegabytesUsage { get; set; }
    }
}
