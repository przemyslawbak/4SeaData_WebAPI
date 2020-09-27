using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class SeaModel
    {
        [Key]
        public int AreaId { get; set; }
        public int? MRGID { get; set; }
        public string Source { get; set; }
        public string PlaceType { get; set; }
        public double? Latitude { get; set; }
        public double? MinLatitude { get; set; }
        public double? MaxLatitude { get; set; }
        public double? Longitude { get; set; }
        public double? MinLongitude { get; set; }
        public double? MaxLongitude { get; set; }
        public double? Precision { get; set; }
        public double? PolygonSize { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string Status { get; set; }
    }
}
