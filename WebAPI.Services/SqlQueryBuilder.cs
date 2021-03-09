using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPI.DAL;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        private readonly IUpdatingProgress _progress;
        private readonly IADORepository _adoRepo;
        private readonly IStringParser _stringParser;

        //todo: unit testing, some day
        public SqlQueryBuilder(IUpdatingProgress progress, IADORepository adoRepo, IStringParser stringParser)
        {
            _progress = progress;
            _adoRepo = adoRepo;
            _stringParser = stringParser;
        }

        public void CreateAndSendUpdatesQuery(List<VesselUpdateModel> updatedVessels)
        {
            StringBuilder completeQuerySb = new StringBuilder();

            foreach (var update in updatedVessels)
            {
                if (IsUpdateValid(update))
                {
                    completeQuerySb.Append(GetVesselUpdateQuery(update));
                }
            }

            _adoRepo.SetUpdates(completeQuerySb.ToString());
        }

        private bool IsUpdateValid(VesselUpdateModel update)
        {
            if (update == null) return false;

            if (NonNullPropertiesCount(update) <= 2) return false;

            return true;
        }

        private int NonNullPropertiesCount(VesselUpdateModel entity)
        {
            return entity.GetType()
                         .GetProperties()
                         .Select(x => x.GetValue(entity, null))
                         .Count(v => v != null);
        }

        private string GetVesselUpdateQuery(VesselUpdateModel update)
        {
            VesselAisUpdateModel existingVessel = FindExistingVessel(update.IMO);
            existingVessel.Imo = update.IMO;

            string query = BuildSingleVesselQuery(existingVessel, update) ?? "";

            return query;
        }

        private VesselAisUpdateModel FindExistingVessel(int? iMO)
        {
            string searchQuery = BuildSearchQuery(iMO);

            return _adoRepo.GetVesselData(searchQuery);
        }

        private string BuildSearchQuery(int? iMO)
        {
            if (_progress.GetReturnedResultsQuantity() < _progress.GetTotalResultsQuantity() / 2)
            {
                return "SELECT TOP 1 MMSI,SpeedMax,DraughtMax FROM dbo.Vessels WHERE IMO = " + iMO + " ORDER BY IMO DESC;";
            }
            else
            {
                return "SELECT TOP 1 MMSI,SpeedMax,DraughtMax FROM dbo.Vessels WHERE IMO = " + iMO + ";";
            }
        }

        private string BuildSingleVesselQuery(VesselAisUpdateModel existingVessel, VesselUpdateModel update)
        {
            StringBuilder vesselQuerySb = new StringBuilder();
            vesselQuerySb.Append("UPDATE dbo.Vessels SET IMO = '" + update.IMO + "'");
            if (existingVessel.Mmsi == 0)
            {
                if (update.MMSI.HasValue)
                {
                    vesselQuerySb.Append(" , MMSI = '" + update.MMSI + "'");
                }
            }
            if (update.AISLatestActivity.HasValue)
            {
                vesselQuerySb.Append(" , AISLatestActivity = '" + update.AISLatestActivity.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
            }
            if (update.ETA.HasValue)
            {
                vesselQuerySb.Append(" , ETA = '" + update.ETA.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
            }
            if (update.Course.HasValue)
            {
                vesselQuerySb.Append(" , Course = '" + update.Course.Value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "'");
            }
            if (update.Lat.HasValue)
            {
                vesselQuerySb.Append(" , Lat = '" + update.Lat.Value.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + "'");
            }
            if (update.Lon.HasValue)
            {
                vesselQuerySb.Append(" , Lon = '" + update.Lon.Value.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture) + "'");
            }
            if (update.Speed.HasValue)
            {
                vesselQuerySb.Append(" , Speed = '" + update.Speed.Value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "'");
                if ((existingVessel.SpeedMax < update.Speed || existingVessel.SpeedMax == null) && update.Speed != 0)
                {
                    vesselQuerySb.Append(" , SpeedMax = '" + update.Speed.Value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "'");
                }
            }
            if (update.Draught.HasValue)
            {
                vesselQuerySb.Append(" , Draught = '" + update.Draught.Value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "'");
                if ((existingVessel.DraughtMax < update.Draught || existingVessel.DraughtMax == null) && update.Draught != 0)
                {
                    vesselQuerySb.Append(" , DraughtMax = '" + update.Draught.Value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "'");
                }
            }
            if (!string.IsNullOrEmpty(update.AISStatus))
            {
                vesselQuerySb.Append(" , AISStatus = '" + _stringParser.MakeFirstCharactersToUpper(update.AISStatus) + "'");
            }
            if (!string.IsNullOrEmpty(update.Destination))
            {
                vesselQuerySb.Append(" , Destination = '" + _stringParser.MakeFirstCharactersToUpper(update.Destination) + "'");
            }
            if (!string.IsNullOrEmpty(update.GeographicalArea))
            {
                vesselQuerySb.Append(" , GeographicalArea = '" + update.GeographicalArea + "'");
            }
            if (!string.IsNullOrEmpty(update.CurrnetPortLocode))
            {
                vesselQuerySb.Append(" , CurrnetPortLocode = '" + update.CurrnetPortLocode + "'");
            }
            vesselQuerySb.Append(" WHERE IMO = " + existingVessel.Imo + "; ");

            vesselQuerySb.AppendLine();

            return vesselQuerySb.ToString();
        }
    }
}
