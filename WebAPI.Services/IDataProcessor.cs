using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IDataProcessor
    {
        Task IterateThroughUpdateObjects(List<VesselAisUpdateModel> updateList);
    }
}