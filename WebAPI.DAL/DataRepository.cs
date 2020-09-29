using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebAPI.Models;
using System.Linq;
using Updater.Models;
using System;
using System.Text.RegularExpressions;
using System.Reflection;

namespace WebAPI.DAL
{
    public class DataRepository : IDataRepository
    {
        private readonly ApplicationDbContext _context;

        public DataRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public void SaveUpdatedVessels(List<VesselModel> updatedVessels)
        {
            foreach (var vessel in updatedVessels)
            {
                VesselModel vsl = _context.Vessels.First(v => v.VesselId == vessel.VesselId);
                VesselModel vsl_updated = vessel;

                foreach (PropertyInfo fromProp in typeof(VesselModel).GetProperties())
                {
                    PropertyInfo toProp = typeof(VesselModel).GetProperty(fromProp.Name);
                    object toValue = toProp.GetValue(vsl_updated, null);
                    if (toValue != null)
                    {
                        fromProp.SetValue(vsl, toValue, null);
                    }
                }
            }

            _context.SaveChanges();
        }

        public List<SeaModel> GetAllSeaAreas()
        {
            return _context.Seas.AsNoTracking().OrderBy(s => s.PolygonSize).ToList();
        }

        public List<VesselAisUpdateModel> GetAllVesselsUpdateModels()
        {
            List<VesselAisUpdateModel> result = (from vessel in _context.Vessels.AsNoTracking()
                          select new VesselAisUpdateModel
                          {
                              Imo = vessel.IMO.Value,
                              Mmsi = vessel.MMSI.Value,
                              Speed = vessel.Speed
                          }).ToList();

            return result;
        }

        public void SaveDatabaseQuantities() //todo: service
        {
            AppSettings settings = GetSettings();

            int emailsQty = _context.Emails.Count();
            int companiesQty = _context.Companies.Count();
            int vesselsQty = _context.Vessels.Count();
            List<string> companiesCountry = GetAllCompaniesCountries();
            List<string> companiesFleet = GetVesselTypes();
            List<string> vesselTypes = GetVesselTypes();
            List<string> vesselFlags = GetVesselFlags();
            List<string> vesselStatus = GetVesselStatus();
            List<string> vesselClasses = GetVesselClasses();
            List<string> vesselBuilders = GetVesselBuilders();
            List<string> vesselRegions = GetVesselRegions();
            List<string> vesselAisStatus = GetVesselAisStatus();
            List<string> vesselDetailedType = GetVesselDetailedTypes();

            settings.TotalVessels = vesselsQty;
            settings.TotalEmails = emailsQty;
            settings.TotalCompanies = companiesQty;
            settings.LastDataUpdate = DateTime.UtcNow;
            settings.CompanyCountry = companiesCountry.Distinct().ToArray();
            settings.CompanyFleetTypes = companiesFleet.Distinct().ToArray();
            settings.VesselType = vesselTypes.Distinct().ToArray();
            settings.VesselFlag = vesselFlags.Distinct().ToArray();
            settings.VesselStatus = vesselStatus.Distinct().ToArray();
            settings.VesselClass = vesselClasses.Distinct().ToArray();
            settings.VesselBuilders = vesselBuilders.Distinct().ToArray();
            settings.VesselRegion = vesselRegions.Distinct().ToArray();
            settings.VesselAisStatus = vesselAisStatus.Distinct().ToArray();
            settings.VesselDetailedType = vesselDetailedType.Distinct().ToArray();

            _context.SaveChanges();
        }

        private List<string> GetVesselDetailedTypes()
        {
            List<string[]> list = _context.Vessels.Select(v => v.DetailedType).Where(v => v != null).ToList();
            List<string> result = list.SelectMany(x => x).Distinct().OrderBy(d => d).ToList();
            return result;
        }

        private List<string> GetVesselAisStatus()
        {
            return _context.Vessels.OrderBy(v => v.AISStatus).Select(v => v.AISStatus).Distinct().ToList();
        }

        private List<string> GetVesselRegions()
        {
            return _context.Vessels.OrderBy(v => v.GeographicalArea).Select(v => v.GeographicalArea).Distinct().ToList();
        }

        private List<string> GetVesselBuilders()
        {
            return _context.Vessels.OrderBy(v => v.BuilderCountry).Select(v => v.BuilderCountry).Distinct().ToList();
        }

        private List<string> GetVesselClasses()
        {
            return _context.Vessels.OrderBy(v => v.Classification).Select(v => v.Classification).Distinct().ToList();
        }

        private List<string> GetVesselStatus()
        {
            return _context.Vessels.OrderBy(v => v.VesselStatus).Select(v => v.VesselStatus).Distinct().ToList();
        }

        private List<string> GetVesselFlags()
        {
            return _context.Vessels.OrderBy(v => v.Flag).Select(v => v.Flag).Distinct().ToList();
        }

        private List<string> GetVesselTypes()
        {
            return _context.Vessels.OrderBy(v => v.VesselCategory).Select(v => v.VesselCategory).Distinct().ToList();
        }

        private List<string> GetAllCompaniesCountries()
        {
            return _context.Companies.OrderBy(c => c.AddressCountry).Select(c => c.AddressCountry).Distinct().ToList();
        }

        private AppSettings GetSettings()
        {
            return _context.Settings.FirstOrDefault();
        }

        private string CleanUpStringCases(string someString) //todo: service
        {
            if (!string.IsNullOrEmpty(someString))
            {
                someString = Regex.Replace(someString.Trim().ToLower(), @"(^\w)|(\s\w)", m => m.Value.ToUpper());
                if (someString.Contains("'"))
                    someString = someString.Replace("'", "`");
            }

            return someString;
        }
    }
}
