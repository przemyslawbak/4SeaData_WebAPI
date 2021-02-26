using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class PortModel
    {
        [Key]
        public string PortLocode { get; set; }
        public DateTime? DataUpdate { get; set; }
        public string CountryCode { get; set; }
        public string LocationCode { get; set; }
        public string NameAscii { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double MinLatitude { get; set; }
        public double MaxLatitude { get; set; }
        public double MinLongitude { get; set; }
        public double MaxLongitude { get; set; }
        public int? AreaMRGID { get; set; }
        public double PolygonSize { get; set; }
        public bool IsAirport { get; set; }
        public bool IsPostalTerminal { get; set; }
        public bool IsRailTerminal { get; set; }
        public bool IsRoadTerminal { get; set; }
        public bool IsFixedInstallation { get; set; }
        public int ContainerFacilityQty { get; set; }
        public double? MaxKnownLOA { get; set; }
        public double? MaxKnownBreadth { get; set; }
        public double? MaxKnownDraught { get; set; }
        public virtual ICollection<VesselPort> VesselsPorts { get; set; }
    }
}
