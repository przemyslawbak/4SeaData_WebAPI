using System;
using System.ComponentModel.DataAnnotations;

namespace Updater.Models
{
    public class AppSettings
    {
        [Key]
        public int Id { get; set; }
        public DateTime? LastDataUpdate { get; set; }
        public int? TotalCompanies { get; set; }
        public int? TotalVessels { get; set; }
        public int? TotalEmails { get; set; }
        public string[] VesselStatus { get; set; }
        public string[] VesselType { get; set; }
        public string[] VesselFlag { get; set; }
        public string[] VesselClass { get; set; }
        public string[] VesselRegion { get; set; }
        public string[] VesselAisStatus { get; set; }
        public string[] VesselDetailedType { get; set; }
        public string[] CompanyCountry { get; set; }
        public string[] CompanyRoles { get => new string[] { "Owner", "Manager" }; }
        public string[] CompanyFleetTypes { get; set; }
        public string[] VesselBuilders { get; set; }
    }
}
