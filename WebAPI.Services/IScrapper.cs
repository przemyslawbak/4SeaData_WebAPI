using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IScrapper
    {
        Task<VesselModel> ScrapSingleVessel(int mmsi, int imo, List<SeaModel> seaAreas);
    }
}