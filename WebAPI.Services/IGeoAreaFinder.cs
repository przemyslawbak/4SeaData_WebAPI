namespace WebAPI.Services
{
    public interface IGeoAreaFinder
    {
        string GetGeographicalArea(double? lat, double? lon);
        string GetPortLocode(double? speed, double? lat, double? lon);
        string VerifyDestinationWithLocode(string destination);
    }
}