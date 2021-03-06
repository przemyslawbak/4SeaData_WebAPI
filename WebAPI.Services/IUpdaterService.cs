﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IUpdaterService
    {
        Task<bool> UpdateSingleVesselAsync(int mmsi, int imo, string searchType);
        StatusModel GetUpdatingStatus();
        bool PauseUpdating();
        Task<bool> StopUpdatingAsync();
    }
}