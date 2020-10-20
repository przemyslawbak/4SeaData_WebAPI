using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class DailyStatisticsModel
    {
        [Key]
        public string Area { get; set; }
        public string VesselCategory { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Moving { get; set; }
        public int Moored { get; set; }
        public int Anchored { get; set; }
    }
}
