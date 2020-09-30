using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IScrapper
    {
        Task<VesselModel> ScrapSingleVesselAsync(int mmsi, int imo, List<SeaModel> seaAreas);
    }
}