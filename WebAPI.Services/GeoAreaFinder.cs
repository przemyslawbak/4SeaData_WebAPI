using WebAPI.Models;

namespace WebAPI.Services
{
    public class GeoAreaFinder : IGeoAreaFinder
    {
        private readonly IMemoryAccess _memoryAccess;

        public GeoAreaFinder(IMemoryAccess memoryAccess)
        {
            _memoryAccess = memoryAccess;
        }

        public string GetGeographicalArea(double? lat, double? lon)
        {
            if (lat.HasValue && lon.HasValue)
            {
                foreach (AreaBboxModel sea in _memoryAccess.GetSeaAreas())
                {
                    MapPointModel point = new MapPointModel() { Lat = double.Parse(lat.ToString()), Lon = double.Parse(lon.ToString()) };

                    if (VerifyPolygon(point, sea))
                        return sea.Name;
                }
            }

            return null;
        }

        public string GetPortLocode(double? speed, double? lat, double? lon)
        {
            if (lat.HasValue && lon.HasValue && speed.HasValue)
            {
                if (speed > 0.5)
                {
                    return null;
                }

                foreach (AreaBboxModel port in _memoryAccess.GetPortAreas())
                {
                    MapPointModel point = new MapPointModel() { Lat = double.Parse(lat.ToString()), Lon = double.Parse(lon.ToString()) };

                    if (VerifyPolygon(point, port))
                        return port.KeyProperty;
                }
            }

            return null;
        }

        private bool VerifyPolygon(MapPointModel point, AreaBboxModel area)
        {
            if (point.Lat > area.MinLatitude && point.Lat < area.MaxLatitude && point.Lon > area.MinLongitude && point.Lon < area.MaxLongitude)
            {
                return true;
            }

            return false;
        }
    }
}
