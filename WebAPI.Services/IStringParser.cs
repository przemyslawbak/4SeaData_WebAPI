﻿using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IStringParser
    {
        string BuildUpdatedVesselInfo(VesselModel result);
    }
}