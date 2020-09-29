using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class Scrapper : IScrapper
    {
        public async Task<VesselModel> ScrapSingleVessel(int mmsi, int imo, List<SeaModel> seaAreas)
        {
            await Task.Delay(100); //todo: implement

            return new VesselModel();
        }
    }
}
