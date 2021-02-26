using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class VesselModel
    {
        [Key]
        public int? IMO { get; set; }
        public DateTime? DatatUpdate { get; set; }
        public string Name { get; set; }
        public int? MMSI { get; set; }
        public string CallSign { get; set; }
        public int? GRT { get; set; }
        public int? DWT { get; set; }
        public string VesselCategory { get; set; }
        public DateTime? VesselTypeSince { get; set; }
        public int? YOB { get; set; }
        public string Shipbuilder { get; set; }
        public string BuilderCountry { get; set; }
        public string BuilderCity { get; set; }
        public string VesselStatus { get; set; }
        public string Classification { get; set; }
        public string Flag { get; set; }
        public string PictureLink { get; set; }
        public string[] PreviousOwners { get; set; }
        public string[] PreviousManagers { get; set; }
        //performances
        public DateTime? PerformancesUpdate { get; set; }
        public string MapLink { get; set; }
        public double? LOA { get; set; }
        public double? Breadth { get; set; } //full/vf
        public double? SpeedMax { get; set; }
        public double? SpeedAverage { get; set; }
        public string[] DetailedType { get; set; }
        public string AisVesselType { get; set; }
        //voyage data
        public DateTime? AISLatestActivity { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string GeographicalArea { get; set; }
        public string AISStatus { get; set; }
        public double? Speed { get; set; }
        public double? Course { get; set; }
        public string Destination { get; set; }
        public double? Draught { get; set; }
        public DateTime? ETA { get; set; }
        public int? OwnerId { get; set; }
        public int? ManagerId { get; set; }
        public string PortCurrent { get; set; }
        public string PortNext { get; set; }
        public bool? FullChecked { get; set; }

        //one-to-many
        [ForeignKey(nameof(OwnerId))]
        public CompanyModel CompanyOwner { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public CompanyModel CompanyManager { get; set; }

        //many-to-many
        public virtual ICollection<VesselPort> VesselsPorts { get; set; }
    }
}
