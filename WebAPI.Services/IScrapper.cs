using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IScrapper
    {
        VesselUpdateModel ScrapSingleVessel(int mmsi, int imo, List<SeaModel> seaAreas);
    }
}