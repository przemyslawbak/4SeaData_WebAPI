using System;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class StringParserTests
    {
        private readonly StringParser _service;

        public StringParserTests()
        {
            _service = new StringParser();
        }

        public static readonly object[][] data =
        {
            new object[] { 9482469, new DateTime(2020, 10, 01), "status", "destination", "9482469 | 01.10.2020 | status | destination"},
            new object[] { 9482469, new DateTime(2020, 10, 01), "status", null, "9482469 | 01.10.2020 | status | (no destination)"},
            new object[] { 9482469, new DateTime(2020, 10, 01), null, "destination", "9482469 | 01.10.2020 | (no status) | destination"},
            new object[] { 9482469, null, "status", "destination", "9482469 | (no activity time) | status | destination"},
        };

        [Theory, MemberData(nameof(data))]
        private void BuildUpdatedVesselInfo_OnSpecificParameters_ReturnsCorrectString(int imo, DateTime? activity, string status, string destination, string expected)
        {
            VesselUpdateModel model = new VesselUpdateModel()
            {
                IMO = imo,
                AISLatestActivity = activity,
                AISStatus = status,
                Destination = destination,
            };

            string result = _service.BuildUpdatedVesselInfo(model);

            Assert.Equal(expected, result);
        }

        [Fact]
        private void BuildUpdatedVesselInfo_OnVesselUpdateModelNull_ReturnsNull()
        {
            string result = _service.BuildUpdatedVesselInfo(null);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("I_have_no_span_2020/10/08_tab", null)]
        [InlineData("I_have_<span>2020/10/08<tab>", "2020/10/08")]
        private void GetTrimmedTime2_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetTrimmedTime2(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("<td class=\"v3 tooltip expand\" data-title=\"2020/10/08 UTC", "2020/10/08")]
        [InlineData("<td class=\"v3 tooltip expand\" data-title=\"2020/10/08_UTC", null)]
        [InlineData("<td class=\"v4 tooltip expand\" data-title=\"2020/10/08 UTC", null)]
        private void GetTrimmedTime1_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetTrimmedTime1(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("Navigational status:</div><div class=<div>some_status</div>", "some_status")]
        [InlineData("Navigational wrong:</div><div class=<div>some_status</div>", null)]
        private void GetAisStatusTrimmed_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetAisStatusTrimmed(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("some_text", "some_text")]
        [InlineData("-", null)]
        private void GetUndashedTrimmedText_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetUndashedTrimmedText(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("125.4° ", "125.4")]
        [InlineData("125.4", null)]
        private void GetTrimmedCourse_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetTrimmedCourse(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("/12.4 k ", "12.4")]
        [InlineData("12.4 k ", null)]
        [InlineData("12.4", null)]
        private void GetTrimmedSpeed_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetTrimmedSpeed(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("12.4 N/12.4 E", "12.4")]
        [InlineData("12.4 N/12.4 W", "-12.4")]
        [InlineData("12.4 / 12.4", null)]
        private void GetTrimmedLongitude_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetTrimmedLongitude(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("12.4 N /", "12.4")]
        [InlineData("12.4 S /", "-12.4")]
        [InlineData("12.4N", null)]
        [InlineData("12.4S", null)]
        [InlineData("12.4 / ", null)]
        private void GetTrimmedLatitude_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetTrimmedLatitude(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("12.4 m", "12.4")]
        [InlineData("12.4m", null)]
        private void GetTrimmedDraught_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetTrimmedDraught(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("<td class=\"v3\">some_text<div>", "some_text")]
        [InlineData("<td class=\"v4\">some_text<div>", null)]
        private void SplitTableRow_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.SplitTableRow(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("MMSI:<td class=\"v3\">some_text<div>11111111<div>", "11111111")]
        [InlineData("MMSI<td class=\"v3\">some_text<div>11111111<div>", null)]
        private void GetTrimmedMmsi_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.GetTrimmedMmsi(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("<td class=\"v3\">", true)]
        [InlineData("<td class=\"v4\">", false)]
        private void IsTableRowCorrect_OnPropeTextParameter_ReturnsProperResult(string text, bool expected)
        {
            bool result = _service.IsTableRowCorrect(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("MMSI:", true)]
        [InlineData("MMSI", false)]
        private void IsContainingMmsi_OnPropeTextParameter_ReturnsProperResult(string text, bool expected)
        {
            bool result = _service.IsContainingMmsi(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("Last seen: <span>", true)]
        [InlineData("Last seen:", false)]
        [InlineData("<span>", false)]
        private void IsRowTimeRow_OnPropeTextParameter_ReturnsProperResult(string text, bool expected)
        {
            bool result = _service.IsRowTimeRow(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("22.3", 22.3)]
        [InlineData("not_double", null)]
        private void ParsedTrimmedNullableDouble_OnPropeTextParameter_ReturnsProperResult(string text, double? expected)
        {
            double? result = _service.ParsedTrimmedNullableDouble(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, 0)]
        [InlineData("22", 22)]
        [InlineData("22.3", 0)]
        [InlineData("not_int", 0)]
        private void ParsedInt_OnPropeTextParameter_ReturnsProperResult(string text, int expected)
        {
            int result = _service.ParsedInt(text);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("some string to process", "Some String To Process")]
        private void MakeFirstCharactersToUpper_OnPropeTextParameter_ReturnsProperResult(string text, string expected)
        {
            string result = _service.MakeFirstCharactersToUpper(text);

            Assert.Equal(expected, result);
        }

        public static readonly object[][] dates =
        {
            new object[] { "2020/10/08", new DateTime(2020, 10, 08)},
            new object[] { null, null},
            new object[] { "not_a_date", null},
        };

        [Theory, MemberData(nameof(dates))]
        private void ParsedTrimmedNullableDateTime_OnSpecificParameters_ReturnsCorrectString(string text, DateTime? expected)
        {
            DateTime? result = _service.ParsedTrimmedNullableDateTime(text);

            Assert.Equal(expected, result);
        }
    }
}
