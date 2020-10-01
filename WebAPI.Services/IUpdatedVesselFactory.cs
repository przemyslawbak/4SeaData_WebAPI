using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IVesselFactory
    {
        Task<VesselUpdateModel> GetVesselUpdatesAsync(VesselAisUpdateModel aisUpdateModel, List<SeaModel> seaAreas, CancellationToken token, SemaphoreSlim semaphoreThrottel);
    }
}