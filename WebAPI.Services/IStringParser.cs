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
        double? ParsedTrimmedNullableDouble(string speed);
        DateTime? ParsedTrimmedNullableDateTime(string eta);
        int ParsedInt(string mmsi);
        string GetTrimmedDraught(string rowText);
        string GetUndashedDestination(string v);
        string GetTrimmedLatitude(string v);
        string GetTrimmedLongitude(string v);
    }
}