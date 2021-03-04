using System;
using System.Collections.Generic;
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

        //todo: unit test
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

        public string VerifyDestinationWithLocode(string destination)
        {
            if (!string.IsNullOrEmpty(destination))
            {
                if (destination.Contains("<"))
                {
                    destination = destination.Split('<')[0];
                }
                if (destination.Contains(">"))
                {
                    destination = destination.Split('>')[0];
                }
                if (destination.Trim().Length == 6 && destination.Contains(" "))
                {
                    destination = destination.Trim().Replace(" ", string.Empty);
                }
                if (destination.Length == 5)
                {
                    destination = TryParseWithLocodes(destination);
                }
            }

            return destination;
        }

        private string TryParseWithLocodes(string destination)
        {
            Dictionary<string, string> portsDict = _memoryAccess.GetPortLocodeNameDictionary();
            if (portsDict.TryGetValue(destination, out string value))
            {
                return value;
            }
            else
            {
                return destination;
            }
        }

        private bool VerifyPolygon(MapPointModel point, AreaBboxModel area)
        {
            //+/- 0.1 for port boundaries
            if (point.Lat > (area.MinLatitude - 0.1) && point.Lat < (area.MaxLatitude + 0.1) && point.Lon > (area.MinLongitude - 0.1) && point.Lon < (area.MaxLongitude + 0.1))
            {
                return true;
            }

            return false;
        }
    }
}
