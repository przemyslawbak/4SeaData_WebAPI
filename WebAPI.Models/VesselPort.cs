using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class VesselPort
    {
        [Key]
        public int VesselId { get; set; }
        public int PortId { get; set; }
        public DateTime? Departure { get; set; }
        public DateTime? Arrival { get; set; }
        [ForeignKey("VesselId")]
        public VesselModel VesselModel { get; set; }
        [ForeignKey("PortId")]
        public PortModel PortModel { get; set; }
    }
}
