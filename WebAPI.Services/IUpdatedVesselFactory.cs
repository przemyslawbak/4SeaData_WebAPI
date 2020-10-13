using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IUpdatedVesselFactory
    {
        Task<VesselUpdateModel> GetVesselUpdatesAsync(VesselAisUpdateModel aisUpdateModel);
    }
}