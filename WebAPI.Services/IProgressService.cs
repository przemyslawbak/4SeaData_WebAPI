using System.Threading;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IProgressService
    {
        StatusModel GetProgressStatus();
        bool GetIsUpdatingStarted();
        bool GetIsUpdatingPaused();
        void SetUpdatingPaused();
        void SetUpdatingUnpaused();
        CancellationToken GetNewCalnellationToken();
        void SetUpdatingStarted();
        void SetUpdatingStopped();
        void SetCompletedUpdatesTime();
        void SetReturnedVessels(int qty);
        CancellationTokenSource GetTokenSource();
        CancellationToken GetCurrentCalnellationToken();
    }
}