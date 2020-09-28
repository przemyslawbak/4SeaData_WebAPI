using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IUpdaterService
    {
        VesselModel GetSingleVessel(int mmsi, int imo, string searchType);
        StatusModel GetUpdatingStatus();
        bool PauseUpdating();
        Task<bool> StartUpdatingAsync();
        Task<bool> StopUpdatingAsync();
    }
}