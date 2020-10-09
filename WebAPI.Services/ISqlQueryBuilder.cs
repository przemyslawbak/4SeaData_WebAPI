using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface ISqlQueryBuilder
    {
        string GetCompleteQuery(List<VesselUpdateModel> updatedVessels);
    }
}