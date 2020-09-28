using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdaterService : IUpdaterService
    {
        private readonly ITimeHostedUpdater _hostedUpdater;
        private readonly IProgressService _progress;

        public UpdaterService(ITimeHostedUpdater hostedUpdater, IProgressService progress)
        {
            _hostedUpdater = hostedUpdater;
            _progress = progress;
        }

        public StatusModel GetUpdatingStatus()
        {
            return _progress.GetProgressStatus();
        }

        public bool PauseUpdating()
        {
            if (_progress.GetIsUpdatingStarted())
            {
                if (_progress.GetIsUpdatingPaused())
                {
                    _progress.SetUpdatingUnpaused();
                }
                else
                {
                    _progress.SetUpdatingPaused();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> StartUpdatingAsync()
        {
            if (_progress.GetIsUpdatingStarted())
            {
                return false;
            }
            else
            {
                await _hostedUpdater.StartAsync(_progress.GetNewCalnellationToken());

                return true;
            }
        }

        public async Task<bool> StopUpdatingAsync()
        {
            if (_progress.GetIsUpdatingStarted())
            {
                await _hostedUpdater.StopAsync(_progress.GetTokenSource());

                return true;
            }
            else
            {
                return false;
            }
        }

        public VesselModel GetSingleVessel(int mmsi, int imo, string searchType)
        {
            throw new System.NotImplementedException(); //todo: implement
        }
    }
}
