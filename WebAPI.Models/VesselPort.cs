using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class VesselPort
    {
        [Key]
        public int Id { get; set; }
        public int IMO { get; set; }
        public string PortLocode { get; set; }
        public DateTime? Departure { get; set; }
        public DateTime? Arrival { get; set; }
        [ForeignKey(nameof(IMO))]
        public VesselModel VesselModel { get; set; }
        [ForeignKey(nameof(PortLocode))]
        public PortModel PortModel { get; set; }
    }
}
