using System;

namespace WebAPI.Models
{
    public class VesselPort
    {
        public int IMO { get; set; }
        public string PortLocode { get; set; }
        public DateTime? Departure { get; set; }
        public DateTime? Arrival { get; set; }
        public VesselModel VesselModel { get; set; }
        public PortModel PortModel { get; set; }
    }
}
