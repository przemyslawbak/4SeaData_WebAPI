namespace WebAPI.Models
{
    public class VesselAisUpdateModel
    {
        public int Mmsi { get; set; }
        public int Imo { get; set; }
        public double? Speed { get; set; }
        public double? SpeedMax { get; set; }
        public double? DraughtMax { get; set; }
    }
}
