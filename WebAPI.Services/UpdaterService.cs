using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdaterService : IUpdaterService
    {
        private readonly IHostedService _hostedUpdater;
        private readonly IProgressService _progress;

        public UpdaterService(IHostedService hostedUpdater, IProgressService progress)
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
            if (_progress.GetIsUpdatingStarted()) //todo: what if waiting for new timer?
            {
                await _hostedUpdater.StopAsync(_progress.GetCurrentCalnellationToken());

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
