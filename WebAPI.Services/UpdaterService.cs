using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdaterService : IUpdaterService
    {
        private readonly IHostedService _hosterUpdater;
        private readonly IProgressService _progress;

        public UpdaterService(IHostedService hostedUpdater, IProgressService progress)
        {
            _hosterUpdater = hostedUpdater;
            _progress = progress;
        }

        public VesselModel GetSingleVessel(int mmsi, int imo, string searchType)
        {
            throw new System.NotImplementedException();
        }

        public StatusModel GetUpdatingStatus()
        {
            throw new System.NotImplementedException();
        }

        public bool PauseUpdating()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> StartUpdatingAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> StopUpdatingAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
