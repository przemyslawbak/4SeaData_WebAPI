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

        public CancellationToken GetCalnellationToken()
        {
            if (CancellationToken != null)
            {
                if (!CancellationToken.IsCancellationRequested)
                {
                    return CancellationToken;
                }
            }

            TokenSource = new CancellationTokenSource();

            return TokenSource.Token;
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

        public bool IsUpdatingStarted()
        {
            return IsUpdatingInProgress;
        }

        public void SetUpdatingPaused()
        {
            IsUpdatingPaused = true;
        }

        public void SetUpdatingUnpaused()
        {
            IsUpdatingPaused = false;
        }

        private float GetProcessInfo()
        {
            Process proc = Process.GetCurrentProcess();
            long mem = proc.WorkingSet64;
            ProcessDataModel process = new ProcessDataModel() { MemoryMegabytesUsage = (mem / 1024f) / 1024f };
            return process.MemoryMegabytesUsage;

        }

        bool IProgressService.IsUpdatingPaused()
        {
            return IsUpdatingPaused;
        }
    }
}
