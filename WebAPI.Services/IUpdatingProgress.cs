using System.Threading;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IUpdatingProgress
    {
        StatusModel GetProgressStatus();
        bool GetIsUpdatingStarted();
        bool GetIsUpdatingPaused();
        void SetUpdatingPaused();
        void SetUpdatingUnpaused();
        void SetUpdatingStarted();
        void SetUpdatingCompleted();
        CancellationToken GetCurrentCalnellationToken();
        void SetTotalResultsQuantities(int count);
        int GetTotalResultsQuantity();
        bool GetIsUpdatingDatabase();
        void AddFailedRequest();
        void SetLastError(string message);
        void SetLastUpdatedVessel(string v);
        void AddToReturnedResultsQuantity();
        void UpdateMissingProperties(VesselUpdateModel updatedVessel);
        void AddSkipped();
        int GetCurrentUpdateStep(int counter, int step);
        int GetReturnedResultsQuantity();
        void SetUpdatingDatabaseTrue();
        void SetUpdatingDatabaseFalse();
    }
}