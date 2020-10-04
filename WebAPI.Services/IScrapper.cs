using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IScrapper
    {
        VesselUpdateModel ScrapSingleVessel(int mmsi, int imo);
    }
}