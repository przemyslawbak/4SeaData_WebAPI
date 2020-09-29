using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface ITaskGenerator
    {
        Task<VesselModel> CreateNewTask(VesselAisUpdateModel aisUpdateModel, List<SeaModel> seaAreas, CancellationToken token, SemaphoreSlim semaphoreThrottel);
    }
}