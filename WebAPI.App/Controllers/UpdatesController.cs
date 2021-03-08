using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.App.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UpdatesController : Controller
    {
        private readonly string _securityKey;
        private readonly IConfiguration _configuration;
        private readonly IUpdaterService _updater;

        public UpdatesController(IConfiguration configuration, IUpdaterService updater)
        {
            _updater = updater;
            _configuration = configuration;
            _securityKey = _configuration["Authentication:Key"];
        }

        /// <summary>
        /// GET: /api/updates/pausing-updates?key=
        /// </summary>
        /// <param name="key">Clients security key.</param>
        [HttpGet]
        [Route("/api/updates/pausing-updates")]
        public IActionResult GetPauseDataUpdating(string key)
        {
            if (_securityKey != key)
            {
                return new ObjectResult("Unauthorized.") { StatusCode = 401 };
            }

            if (!_updater.PauseUpdating())
            {
                return new ObjectResult("Updating is not started yet.") { StatusCode = 417 };
            }

            return Ok();
        }

        /// <summary>
        /// GET: /api/updates/stop-updates?key=
        /// </summary>
        /// <param name="key">Clients security key.</param>
        [HttpGet]
        [Route("/api/updates/stop-updates")]
        public async Task<IActionResult> GetStopDataUpdatingAsync(string key)
        {
            if (_securityKey != key)
            {
                return new ObjectResult("Unauthorized.") { StatusCode = 401 };
            }

            if (!await _updater.StopUpdatingAsync())
            {
                return new ObjectResult("Failed to stop updating.") { StatusCode = 417 };
            }

            return Ok();
        }

        /// <summary>
        /// GET: /api/updates/get-status?key=
        /// </summary>
        /// <param name="key">Clients security key.</param>
        [HttpGet]
        [Route("/api/updates/get-status")]
        public IActionResult GetUpdatingStatus(string key)
        {
            if (_securityKey != key)
            {
                return new ObjectResult("Unauthorized.") { StatusCode = 401 };
            }

            StatusModel status = _updater.GetUpdatingStatus();

            return Json(status);
        }

        /// <summary>
        /// GET: api/updates/update-vessel
        /// </summary>
        /// <param name="key">Clients security key.</param>
        /// <param name="mmsi">Vessel MMSI number.</param>
        /// <param name="imo">Vessel IMO number.</param>
        /// <param name="searchType">Vessel search type.</param>
        [HttpGet("/api/updates/update-vessel")]
        public async Task<IActionResult> UpdateSingleVesselAsync(string key, int mmsi, int imo, string searchType)
        {
            if (_securityKey != key)
            {
                return new ObjectResult("Unauthorized.") { StatusCode = 401 };
            }

            bool result = await _updater.UpdateSingleVesselAsync(mmsi, imo, searchType);

            if (!result)
            {
                return new ObjectResult("Failed to update vessel.") { StatusCode = 417 };
            }

            return Ok();
        }
    }
}
