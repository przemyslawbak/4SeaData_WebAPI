using System;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IStringParser
    {
        string BuildUpdatedVesselInfo(VesselUpdateModel result);
        string SplitRow(string v);
        string GetXpath(string name);
        string GetTrimmedCourse(string rowText);
        string GetTrimmedText(string rowText);
        bool IsTableRowCorrect(string v);
        string GetTrimmedSpeed(string rowText);
        double? ParsedNullableDouble(string speed);
        DateTime? ParsedNullableDateTime(string eta);
        int ParsedInt(string mmsi);
    }
}