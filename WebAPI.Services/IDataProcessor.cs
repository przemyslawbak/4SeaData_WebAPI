using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IDataProcessor
    {
        Task UpdateListOfVesselsAsync(List<VesselAisUpdateModel> updateList);
        Task<bool> UpdateSingleVesselAsync(int mmsi, int imo, string searchType);
    }
}