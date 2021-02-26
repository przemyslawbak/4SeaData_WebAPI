using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IMemoryAccess
    {
        List<AreaBboxModel> GetSeaAreas();
        List<AreaBboxModel> GetPortAreas();
    }
}