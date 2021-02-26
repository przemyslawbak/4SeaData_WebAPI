namespace WebAPI.Models
{
    public class AreaBboxModel
    {
        public string KeyProperty { get; set; }
        public string Name { get; set; }
        public double? MinLatitude { get; set; }
        public double? MaxLatitude { get; set; }
        public double? MinLongitude { get; set; }
        public double? MaxLongitude { get; set; }
    }
}
