using System;

namespace WebAPI.Models
{
    public class VesselUpdateModel
    {
        public int IMO { get; set; }
        public int? MMSI { get; set; }
        public string Destination { get; set; }
        public double? Draught { get; set; }
        public double? Speed { get; set; }
        public double? Course { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public DateTime? AISLatestActivity { get; set; }
        public DateTime? ETA { get; set; }
        public string AISStatus { get; set; }
        public string GeographicalArea { get; set; }
    }
}
