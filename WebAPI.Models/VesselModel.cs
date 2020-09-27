using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class VesselModel
    {
        [Key]
        public int VesselId { get; set; } //
        public int? IMO { get; set; } //repo
        public DateTime? DatatUpdate { get; set; } //repo
        public string Name { get; set; } //repo
        public int? MMSI { get; set; } //repo
        public string CallSign { get; set; } //full/vf
        public int? GRT { get; set; } //full/vf/
        public int? DWT { get; set; } //full/vf/
        public string VesselCategory { get; set; } //
        public DateTime? VesselTypeSince { get; set; } //-no display
        public int? YOB { get; set; } //full/vf
        public string Shipbuilder { get; set; } //
        public string BuilderCountry { get; set; } //repo
        public string BuilderCity { get; set; } //repo
        public string VesselStatus { get; set; } //
        public string Classification { get; set; } //
        public string Flag { get; set; } //full/vf
        public string PictureLink { get; set; } //-no display
        public string[] PreviousOwners { get; set; } //-no display
        public string[] PreviousManagers { get; set; } //-no display
        //performances
        public DateTime? PerformancesUpdate { get; set; } //-no display
        public string MapLink { get; set; } //-no display
        public double? LOA { get; set; } //full/vf
        public double? Breadth { get; set; } //full/vf
        public double? SpeedMax { get; set; } //repo/
        public double? SpeedAverage { get; set; } //repo/
        public string[] DetailedType { get; set; } //full/vf/sp
        public string AisVesselType { get; set; } //full/vf
        //voyage data
        public DateTime? AISLatestActivity { get; set; } //basic/vf
        public double? Lat { get; set; } //basic/vf
        public double? Lon { get; set; } //basic/vf
        public string GeographicalArea { get; set; } //repo/
        public string AISStatus { get; set; } //basic/vt
        public double? Speed { get; set; } //basic/vf
        public double? Course { get; set; } //basic/vf
        public string Destination { get; set; } //basic/vf
        public double? Draught { get; set; } //basic/vf
        public DateTime? ETA { get; set; } //basic/vf
        public int? OwnerId { get; set; } //repo/
        public int? ManagerId { get; set; } //repo/
        public string PortCurrent { get; set; } //-no display
        public string PortNext { get; set; } //-no display
        public bool? FullChecked { get; set; } //-no display

        //one-to-many
        [ForeignKey(nameof(OwnerId))]
        public CompanyModel CompanyOwner { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public CompanyModel CompanyManager { get; set; }
    }
}
