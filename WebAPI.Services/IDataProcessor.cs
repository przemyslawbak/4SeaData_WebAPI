using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IDataProcessor
    {
        Task IterateThroughObjects(List<VesselAisUpdateModel> updateList, List<SeaModel> seaAreas);
    }
}