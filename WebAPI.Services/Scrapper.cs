using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class Scrapper : IScrapper
    {
        public async Task<VesselModel> ScrapSingleVesselAsync(int mmsi, int imo, List<SeaModel> seaAreas)
        {
            await Task.Delay(1000); //todo: implement

            return new VesselModel();
        }
    }
}
