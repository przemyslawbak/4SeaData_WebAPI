using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdaterService : IUpdaterService
    {
        private readonly IHostedService _hostedUpdater;
        private readonly IProgressService _progress;
        private readonly IDataProcessor _dataProcessor;

        public UpdaterService(IHostedService hostedUpdater, IProgressService progress, IDataProcessor dataProcessor)
        {
            _hostedUpdater = hostedUpdater;
            _progress = progress;
            _dataProcessor = dataProcessor;
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

        public async Task<bool> StopUpdatingAsync()
        {
            if (_progress.GetIsUpdatingStarted())
            {
                await _hostedUpdater.StopAsync(_progress.GetCurrentCalnellationToken());

                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateSingleVesselAsync(int mmsi, int imo, string searchType)
        {
            return await _dataProcessor.UpdateSingleVesselAsync(mmsi, imo, searchType);
        }
    }
}
