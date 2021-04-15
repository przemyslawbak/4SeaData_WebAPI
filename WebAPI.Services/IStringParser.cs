using System;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IStringParser
    {
        string BuildUpdatedVesselInfo(VesselUpdateModel result);
        string SplitTableRow(string text);
        string GetXpath(string text);
        string GetTrimmedCourse(string text);
        string GetUndashedTrimmedText(string text);
        bool IsTableRowCorrect(string text);
        string GetTrimmedSpeed(string text);
        double? ParsedTrimmedNullableDouble(string text);
        DateTime? ParsedTrimmedNullableDateTime(string text);
        int ParsedInt(string text);
        string GetTrimmedDraught(string text);
        string GetTrimmedLatitude(string text);
        string GetTrimmedLongitude(string text);
        bool IsContainingMmsi(string text);
        string GetAisStatusTrimmed(string text);
        string GetTrimmedTime1(string node);
        bool IsRowTimeRow(string innerHtml);
        string GetTrimmedTime2(string innerHtml);
        string MakeFirstCharactersToUpper(string aISStatus);
        string GetTrimmedMmsi(string innerHtml);
        string GetTrimmedDestination(string node);
        string GetTrimmedEta(string node);
    }
}