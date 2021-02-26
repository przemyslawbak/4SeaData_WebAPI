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
                foreach (SeaModel area in _memoryAccess.GetSeaAreas())
                {
                    MapPointModel point = new MapPointModel() { Lat = double.Parse(lat.ToString()), Lon = double.Parse(lon.ToString()) };

                    if (VerifyPolygon(point, area))
                        return area.Name;
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

                foreach (PortModel port in _memoryAccess.GetAllPorts())
                {
                    MapPointModel point = new MapPointModel() { Lat = double.Parse(lat.ToString()), Lon = double.Parse(lon.ToString()) };

                    if (VerifyPolygon(point, port))
                        return port.PortLocode;
                }
            }

            return null;
        }

        //todo: DRY
        private bool VerifyPolygon(MapPointModel point, PortModel area)
        {
            if (point.Lat > area.MinLatitude && point.Lat < area.MaxLatitude && point.Lon > area.MinLongitude && point.Lon < area.MaxLongitude)
            {
                return true;
            }

            return false;
        }

        //todo: DRY
        private bool VerifyPolygon(MapPointModel point, SeaModel area)
        {
            if (point.Lat > area.MinLatitude && point.Lat < area.MaxLatitude && point.Lon > area.MinLongitude && point.Lon < area.MaxLongitude)
            {
                return true;
            }

            return false;
        }
    }
}
