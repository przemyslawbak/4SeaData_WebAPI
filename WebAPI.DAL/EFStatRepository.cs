using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.DAL
{
    //todo: DRY
    public class EFStatRepository : IEFStatRepository
    {
        private readonly ApplicationDbContext _context;

        public EFStatRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public int GetCargoMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Cargo" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetCargoNotMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Cargo" && (v.AISStatus == "Moored" || v.AISStatus == "Grounded"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetCargoAnchored(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Cargo" && (v.AISStatus == "Anchored"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetDredgingMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Dredging" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetDredgingNotMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Dredging" && (v.AISStatus == "Moored" || v.AISStatus == "Grounded"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetDredgingAnchored(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Dredging" && (v.AISStatus == "Anchored"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetFishingMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Fishing" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetFishingNotMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Fishing" && (v.AISStatus == "Moored" || v.AISStatus == "Grounded"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetFishingAnchored(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Fishing" && (v.AISStatus == "Anchored"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetOffshoreMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Offshore" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetOffshoreNotMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Offshore" && (v.AISStatus == "Moored" || v.AISStatus == "Grounded"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetOffshoreAnchored(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Offshore" && (v.AISStatus == "Anchored"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetOtherMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Other" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetOtherNotMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Other" && (v.AISStatus == "Moored" || v.AISStatus == "Grounded"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetOtherAnchored(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Other" && (v.AISStatus == "Anchored"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetPassengerMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Passenger" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetPassengerNotMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Passenger" && (v.AISStatus == "Moored" || v.AISStatus == "Grounded"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetPassengerAnchored(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Passenger" && (v.AISStatus == "Anchored"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetTankerMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Tanker" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetTankerNotMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Tanker" && (v.AISStatus == "Moored" || v.AISStatus == "Grounded"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetTankerAnchored(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Tanker" && (v.AISStatus == "Anchored"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetTugMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Tug" &&
            (v.AISStatus == "Constrained By Draught" ||
            v.AISStatus == "Fishing" ||
            v.AISStatus == "Not Under Command" ||
            v.AISStatus == "Restricted Movement" ||
            v.AISStatus == "Under Way" ||
            v.AISStatus == "Sailing" ||
            v.AISStatus == "Moving"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetTugNotMoving(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Tug" && (v.AISStatus == "Moored" || v.AISStatus == "Grounded"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public int GetTugAnchored(string areaName)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v => v.VesselCategory == "Tug" && (v.AISStatus == "Anchored"));

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            return list.Count();
        }

        public void SaveStatistics(DailyStatisticsModel updateStats)
        {
            _context.Statistics.Add(updateStats);

            _context.SaveChanges();
        }

        public int GetMoving(string areaName, string category)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v =>
                v.AISStatus == "Constrained By Draught" ||
                v.AISStatus == "Fishing" ||
                v.AISStatus == "Not Under Command" ||
                v.AISStatus == "Restricted Movement" ||
                v.AISStatus == "Under Way" ||
                v.AISStatus == "Sailing" ||
                v.AISStatus == "Moving");

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            if (category != "All types")
            {
                list = list.Where(v => v.VesselCategory == category);
            }

            return list.Count();
        }

        public int GetMoored(string areaName, string category)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v =>
               v.AISStatus == "Moored" || v.AISStatus == "Grounded");

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            if (category != "All types")
            {
                list = list.Where(v => v.VesselCategory == category);
            }

            return list.Count();
        }

        public int GetAnchored(string areaName, string category)
        {
            IQueryable<VesselModel> list = _context.Vessels.Where(v =>
               v.AISStatus == "Anchored");

            if (areaName != "All areas")
            {
                list = list.Where(v => v.GeographicalArea == areaName);
            }

            if (category != "All types")
            {
                list = list.Where(v => v.VesselCategory == category);
            }

            return list.Count();
        }

        public bool AreCompleteStatsForToday()
        {
            return _context.Statistics.Where(s => s.Date >= DateTime.UtcNow.Date).Count() == 808;
        }

        public void DeleteStats(List<DailyStatisticsModel> statsToBeRemoved)
        {
            _context.Statistics.RemoveRange(statsToBeRemoved);

            _context.SaveChanges();
        }

        public List<DailyStatisticsModel> GetAllStatsForToday()
        {
            return _context.Statistics.Where(s => s.Date >= DateTime.UtcNow.Date).ToList();
        }
    }
}
