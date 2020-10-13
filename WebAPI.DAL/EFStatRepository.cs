using System.Linq;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class EFStatRepository : IEFStatRepository
    {
        private readonly ApplicationDbContext _context;

        public EFStatRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public int GetCargoExpired()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Cargo" && v.AISStatus == "(out-of-date)").Count();
        }

        public int GetCargoMissing()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Cargo" && (v.AISStatus == "" || v.AISStatus == "Undefined" || v.AISStatus == "N/a")).Count();
        }

        public int GetCargoMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Cargo" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving")).Count();
        }

        public int GetCargoNotMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Cargo" && (v.AISStatus == "Anchored" || v.AISStatus == "Moored" || v.AISStatus == "Grounded")).Count();
        }

        public int GetDredgingExpired()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Dredging" && v.AISStatus == "(out-of-date)").Count();
        }

        public int GetDredgingMissing()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Dredging" && (v.AISStatus == "" || v.AISStatus == "Undefined" || v.AISStatus == "N/a")).Count();
        }

        public int GetDredgingMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Dredging" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving")).Count();
        }

        public int GetDredgingNotMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Dredging" && (v.AISStatus == "Anchored" || v.AISStatus == "Moored" || v.AISStatus == "Grounded")).Count();
        }

        public int GetFishingExpired()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Fishing" && v.AISStatus == "(out-of-date)").Count();
        }

        public int GetFishingMissing()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Fishing" && (v.AISStatus == "" || v.AISStatus == "Undefined" || v.AISStatus == "N/a")).Count();
        }

        public int GetFishingMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Fishing" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving")).Count();
        }

        public int GetFishingNotMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Fishing" && (v.AISStatus == "Anchored" || v.AISStatus == "Moored" || v.AISStatus == "Grounded")).Count();
        }

        public int GetOffshoreExpired()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Offshore" && v.AISStatus == "(out-of-date)").Count();
        }

        public int GetOffshoreMissing()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Offshore" && (v.AISStatus == "" || v.AISStatus == "Undefined" || v.AISStatus == "N/a")).Count();
        }

        public int GetOffshoreMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Offshore" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving")).Count();
        }

        public int GetOffshoreNotMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Offshore" && (v.AISStatus == "Anchored" || v.AISStatus == "Moored" || v.AISStatus == "Grounded")).Count();
        }

        public int GetOtherExpired()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Other" && v.AISStatus == "(out-of-date)").Count();
        }

        public int GetOtherMissing()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Other" && (v.AISStatus == "" || v.AISStatus == "Undefined" || v.AISStatus == "N/a")).Count();
        }

        public int GetOtherMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Other" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving")).Count();
        }

        public int GetOtherNotMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Other" && (v.AISStatus == "Anchored" || v.AISStatus == "Moored" || v.AISStatus == "Grounded")).Count();
        }

        public int GetPassengerExpired()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Passenger" && v.AISStatus == "(out-of-date)").Count();
        }

        public int GetPassengerMissing()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Passenger" && (v.AISStatus == "" || v.AISStatus == "Undefined" || v.AISStatus == "N/a")).Count();
        }

        public int GetPassengerMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Passenger" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving")).Count();
        }

        public int GetPassengerNotMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Passenger" && (v.AISStatus == "Anchored" || v.AISStatus == "Moored" || v.AISStatus == "Grounded")).Count();
        }

        public int GetTankerExpired()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Tanker" && v.AISStatus == "(out-of-date)").Count();
        }

        public int GetTankerMissing()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Tanker" && (v.AISStatus == "" || v.AISStatus == "Undefined" || v.AISStatus == "N/a")).Count();
        }

        public int GetTankerMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Tanker" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving")).Count();
        }

        public int GetTankerNotMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Tanker" && (v.AISStatus == "Anchored" || v.AISStatus == "Moored" || v.AISStatus == "Grounded")).Count();
        }

        public int GetTugExpired()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Tug" && v.AISStatus == "(out-of-date)").Count();
        }

        public int GetTugMissing()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Tug" && (v.AISStatus == "" || v.AISStatus == "Undefined" || v.AISStatus == "N/a")).Count();
        }

        public int GetTugMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Tug" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving")).Count();
        }

        public int GetTugNotMoving()
        {
            return _context.Vessels.Where(v => v.VesselCategory == "Tug" && (v.AISStatus == "Anchored" || v.AISStatus == "Moored" || v.AISStatus == "Grounded")).Count();
        }

        public void SaveStatistics(DailyStatisticsModel updateStats)
        {
            _context.Statistics.Add(updateStats);

            _context.SaveChanges();
        }

    }
}
