using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IMemoryAccess
    {
        IEnumerable<SeaModel> GetSeaAreas();
    }
}