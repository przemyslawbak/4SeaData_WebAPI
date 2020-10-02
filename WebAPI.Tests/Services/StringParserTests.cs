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
    }
}
