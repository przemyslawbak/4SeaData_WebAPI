using System;
using System.Diagnostics;
using System.Threading;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class ProgressService : IProgressService
    {
        public bool UpdatingDatabase { get; set; }
        public DateTime LastStartedTime { get; set; }
        public DateTime LastCompletedTime { get; set; }
        public DateTime NextPlannedTime { get; set; }
        public bool IsUpdatingPaused { get; set; }
        public bool IsUpdatingInProgress { get; set; }
        public bool Finalizing { get; set; }
        public int FailedResultsQuantity { get; set; }
        public int TotalResultsQuantity { get; set; }
        public int ReurnedVesselsInCurrent { get; set; }
        public string LastUpdatedVessel { get; set; }
        public string LastError { get; set; }
        public CancellationTokenSource TokenSource { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public CancellationToken GetNewCalnellationToken() //todo: write unit tests
        {
            TokenSource = new CancellationTokenSource();

            return TokenSource.Token;
        }

        public CancellationTokenSource GetTokenSource() //todo: write unit tests
        {
            return TokenSource;
        }

        public StatusModel GetProgressStatus()
        {
            return new StatusModel()
            {
                LastStartedTime = LastStartedTime,
                LastCompletedTime = LastCompletedTime,
                NextPlannedTime = NextPlannedTime,
                IsUpdatingInProgress = IsUpdatingInProgress,
                IsUpdatingPaused = IsUpdatingPaused,
                FailedResultsQuantity = FailedResultsQuantity,
                TotalResultsQuantity = TotalResultsQuantity,
                ReurnedVesselsInCurrent = ReurnedVesselsInCurrent,
                LastUpdatedVessel = LastUpdatedVessel,
                Finalizing = Finalizing,
                UpdatingDatabase = UpdatingDatabase,
                LastError = LastError,
                MemoryMegabytesUsage = GetProcessInfo()
            };
        }

        public bool GetIsUpdatingStarted()
        {
            return IsUpdatingInProgress;
        }

        public void SetCompletedUpdatesTime()
        {
            LastCompletedTime = DateTime.UtcNow;
        }

        public void SetReturnedVessels(int qty)
        {
            ReurnedVesselsInCurrent = qty;
        }

        public void SetUpdatingPaused()
        {
            this.IsUpdatingPaused = true;
        }

        public void SetUpdatingStarted()
        {
            IsUpdatingInProgress = true;
            LastStartedTime = DateTime.UtcNow;
            NextPlannedTime = LastStartedTime.AddMinutes(2);
        }

        public void SetUpdatingStopped()
        {
            IsUpdatingInProgress = false;
        }

        public void SetUpdatingUnpaused()
        {
            IsUpdatingPaused = false;
        }

        public bool GetIsUpdatingPaused()
        {
            return IsUpdatingPaused;
        }

        private float GetProcessInfo()
        {
            Process proc = Process.GetCurrentProcess();
            long mem = proc.WorkingSet64;
            ProcessDataModel process = new ProcessDataModel() { MemoryMegabytesUsage = (mem / 1024f) / 1024f };
            return process.MemoryMegabytesUsage;

        }
    }
}
