using WebAPI.Models;

namespace WebAPI.Services
{
    public class GeoAreaFinder : IGeoAreaFinder
    {
        public string GetGeographicalArea(double? lat, double? lon)
        {
            string result = "";

            foreach (SeaModel area in areas) //todo: caching
            {
                MapPointModel point = new MapPointModel() { Lat = double.Parse(lat.ToString()), Lon = double.Parse(lon.ToString()) };

                if (VerifyPolygon(point, area))
                    return area.Name;
            }

            return result;
        }

        private bool VerifyPolygon(MapPointModel point, SeaModel area)
        {
            if (point.Lat > area.MinLatitude && point.Lat < area.MaxLatitude && point.Lon > area.MinLongitude && point.Lon < area.MaxLongitude)
                return true;

            return false;
        }
    }
}
