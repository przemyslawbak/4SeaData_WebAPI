using System;
using System.Diagnostics;
using System.Threading;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdatingProgress : IUpdatingProgress
    {
        private bool _updatingDatabase;
        private DateTime _lastStartedTime;
        private DateTime _lastCompletedTime;
        private DateTime _nextPlannedTime;
        private bool _isUpdatingPaused;
        private bool _isUpdatingInProgress;
        private int _failedResultsQuantity;
        private int _totalResultsQuantity;
        private int _returnedResultsQuantity;
        private int _skippedResultsQuantity;
        private string _lastUpdatedVessel;
        private string _lastError;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _cancellationToken;

        private int _missingLatCounter;
        private int _missingLonCounter;
        private int _missingDestinationCounter;
        private int _missingDraughtCounter;
        private int _missingSpeedCounter;
        private int _missingCourseCounter;
        private int _missingActivityCounter;
        private int _missingEtaCounter;
        private int _missingStatusCounter;
        private int _missingAreaCounter;

        public CancellationToken GetCurrentCalnellationToken()
        {
            return _cancellationToken;
        }

        public StatusModel GetProgressStatus()
        {
            return new StatusModel()
            {
                MissingActivityTimes = _missingActivityCounter,
                MissingAreas = _missingAreaCounter,
                MissingCourses = _missingCourseCounter,
                MissingDestinations = _missingDestinationCounter,
                MissingDraughts = _missingDraughtCounter,
                MissingEtas = _missingEtaCounter,
                MissingLats = _missingLatCounter,
                MissingLongs = _missingLonCounter,
                MissingSpeeds = _missingSpeedCounter,
                MissingStatuses = _missingStatusCounter,
                LastStartedTime = _lastStartedTime,
                LastCompletedTime = _lastCompletedTime,
                NextPlannedTime = _nextPlannedTime,
                IsUpdatingInProgress = _isUpdatingInProgress,
                IsUpdatingPaused = _isUpdatingPaused,
                FailedResultsQuantity = _failedResultsQuantity,
                TotalResultsQuantity = _totalResultsQuantity,
                ReurnedVesselsInCurrent = _returnedResultsQuantity,
                LastUpdatedVessel = _lastUpdatedVessel,
                UpdatingDatabase = _updatingDatabase,
                LastError = _lastError,
                MemoryMegabytesUsage = GetProcessMemoryUsed(),
                SkippedResultsQuantity = _skippedResultsQuantity
            };
        }

        public bool GetIsUpdatingStarted()
        {
            return _isUpdatingInProgress;
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
            ResetMissingCounters();
        }

        public void SetUpdatingCompleted()
        {
            _isUpdatingInProgress = false;
            _lastCompletedTime = DateTime.UtcNow;
        }

        public void SetUpdatingUnpaused()
        {
            _isUpdatingPaused = false;
        }

        public bool GetIsUpdatingPaused()
        {
            return _isUpdatingPaused;
        }

        public void SetTotalResultsQuantities(int qty)
        {
            _lastUpdatedVessel = string.Empty;
            _returnedResultsQuantity = 0;
            _failedResultsQuantity = 0;
            _skippedResultsQuantity = 0;
            _totalResultsQuantity = qty;
        }

        public int GetTotalResultsQuantity()
        {
            return _totalResultsQuantity;
        }

        public bool GetIsUpdatingDatabase()
        {
            return _updatingDatabase;
        }

        public void AddFailedRequest()
        {
            _failedResultsQuantity++;
        }

        public void SetLastError(string message)
        {
            _lastError = message;
        }

        public void SetLastUpdatedVessel(string vessel)
        {
            if (!string.IsNullOrEmpty(vessel))
            {
                _lastUpdatedVessel = vessel;
            }
        }

        public void AddToReturnedResultsQuantity()
        {
            _returnedResultsQuantity++;
        }

        public void UpdateMissingProperties(VesselUpdateModel updatedVessel)
        {
            if (updatedVessel != null)
            {
                if (!updatedVessel.Lat.HasValue) _missingLatCounter++;
                if (!updatedVessel.Lon.HasValue) _missingLonCounter++;
                if (!updatedVessel.Draught.HasValue) _missingDraughtCounter++;
                if (!updatedVessel.Speed.HasValue) _missingSpeedCounter++;
                if (!updatedVessel.Course.HasValue) _missingCourseCounter++;
                if (!updatedVessel.AISLatestActivity.HasValue) _missingActivityCounter++;
                if (!updatedVessel.ETA.HasValue) _missingEtaCounter++;
                if (string.IsNullOrEmpty(updatedVessel.Destination)) _missingDestinationCounter++;
                if (string.IsNullOrEmpty(updatedVessel.AISStatus)) _missingStatusCounter++;
                if (string.IsNullOrEmpty(updatedVessel.GeographicalArea)) _missingAreaCounter++;
            }
        }

        private void ResetMissingCounters()
        {
            _missingLatCounter = 0;
            _missingLonCounter = 0;
            _missingDestinationCounter = 0;
            _missingDraughtCounter = 0;
            _missingSpeedCounter = 0;
            _missingCourseCounter = 0;
            _missingActivityCounter = 0;
            _missingEtaCounter = 0;
            _missingStatusCounter = 0;
            _missingAreaCounter = 0;
        }

        private float GetProcessMemoryUsed()
        {
            Process proc = Process.GetCurrentProcess();
            long mem = proc.WorkingSet64;
            ProcessDataModel process = new ProcessDataModel() { MemoryMegabytesUsage = (mem / 1024f) / 1024f };
            return process.MemoryMegabytesUsage;
        }

        public void AddSkipped()
        {
            _skippedResultsQuantity++;
        }

        public int GetCurrentUpdateStep(int counter, int step)
        {
            if (counter + step >= GetTotalResultsQuantity())
            {
                return GetTotalResultsQuantity();
            }
            else
            {
                return counter + step;
            }
        }

        public int GetReturnedResultsQuantity()
        {
            return _returnedResultsQuantity;
        }

        public void SetUpdatingDatabaseTrue()
        {
            _updatingDatabase = true;
        }

        public void SetUpdatingDatabaseFalse()
        {
            _updatingDatabase = false;
        }
    }
}
