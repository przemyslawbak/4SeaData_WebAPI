using System.Collections.Generic;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface ISqlQueryBuilder
    {
        void CreateAndSendUpdatesQuery(List<VesselUpdateModel> updatedVessels);
    }
}