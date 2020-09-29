using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IHttpService _http;

        public DataProcessor(IHttpService http)
        {
            _http = http;
        }

        public Task IterateThroughObjects(List<VesselAisUpdateModel> updateList, List<SeaModel> seaAreas)
        {
            throw new System.NotImplementedException();
        }
    }
}
