using System;
using System.Diagnostics;
using System.Threading;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class ProgressService : IProgressService
    {
        private bool _updatingDatabase;
        private DateTime _lastStartedTime;
        private DateTime _lastCompletedTime;
        private DateTime _nextPlannedTime;
        private bool _isUpdatingPaused;
        private bool _isUpdatingInProgress;
        private bool _finalizing;
        private int _failedResultsQuantity;
        private int _totalResultsQuantity;
        private int _reurnedVesselsInCurrent;
        private string _lastUpdatedVessel;
        private string _lastError;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _cancellationToken;

        public CancellationToken GetNewCalnellationToken()
        {
            _tokenSource = new CancellationTokenSource();

            return _tokenSource.Token;
        }

        public CancellationToken GetCurrentCalnellationToken()
        {
            return _cancellationToken;
        }

        public CancellationTokenSource GetTokenSource()
        {
            return _tokenSource;
        }

        public StatusModel GetProgressStatus()
        {
            return new StatusModel()
            {
                LastStartedTime = _lastStartedTime,
                LastCompletedTime = _lastCompletedTime,
                NextPlannedTime = _nextPlannedTime,
                IsUpdatingInProgress = _isUpdatingInProgress,
                IsUpdatingPaused = _isUpdatingPaused,
                FailedResultsQuantity = _failedResultsQuantity,
                TotalResultsQuantity = _totalResultsQuantity,
                ReurnedVesselsInCurrent = _reurnedVesselsInCurrent,
                LastUpdatedVessel = _lastUpdatedVessel,
                Finalizing = _finalizing,
                UpdatingDatabase = _updatingDatabase,
                LastError = _lastError,
                MemoryMegabytesUsage = GetProcessMemoryUsed()
            };
        }

        public bool GetIsUpdatingStarted()
        {
            return _isUpdatingInProgress;
        }

        public void SetCompletedUpdatesTime()
        {
            _lastCompletedTime = DateTime.UtcNow;
        }

        public void SetReturnedVessels(int qty)
        {
            _reurnedVesselsInCurrent = qty;
        }

        public void SetUpdatingPaused()
        {
            _isUpdatingPaused = true;
        }

        public void SetUpdatingStarted()
        {
            _isUpdatingInProgress = true;
            _lastStartedTime = DateTime.UtcNow;
            _nextPlannedTime = _lastStartedTime.AddMinutes(2);
        }

        public void SetUpdatingStopped()
        {
            _isUpdatingInProgress = false;
        }

        public void SetUpdatingUnpaused()
        {
            _isUpdatingPaused = false;
        }

        public bool GetIsUpdatingPaused()
        {
            return _isUpdatingPaused;
        }

        private float GetProcessMemoryUsed()
        {
            Process proc = Process.GetCurrentProcess();
            long mem = proc.WorkingSet64;
            ProcessDataModel process = new ProcessDataModel() { MemoryMegabytesUsage = (mem / 1024f) / 1024f };
            return process.MemoryMegabytesUsage;

        }
    }
}
