namespace WebAPI.Services
{
    public interface IGeoAreaFinder
    {
        string GetGeographicalArea(double? lat, double? lon);
    }
}