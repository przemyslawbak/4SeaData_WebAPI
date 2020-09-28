using System.Threading;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IProgressService
    {
        StatusModel GetProgressStatus();
        bool IsUpdatingStarted();
        bool IsUpdatingPaused();
        void SetUpdatingPaused();
        void SetUpdatingUnpaused();
        CancellationToken GetCalnellationToken();
    }
}