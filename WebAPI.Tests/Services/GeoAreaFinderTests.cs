using Moq;
using System.Collections.Generic;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class GeoAreaFinderTests
    {
        private readonly Mock<IMemoryAccess> _memoryAccessMock;
        private readonly GeoAreaFinder _service;

        public GeoAreaFinderTests()
        {
            _memoryAccessMock = new Mock<IMemoryAccess>();

            _memoryAccessMock.Setup(mock => mock.GetSeaAreas()).Returns(new List<SeaModel>() { new SeaModel() { MaxLatitude = 10.00, MaxLongitude = 10.00, MinLatitude = 00.00, MinLongitude = 00.00, Name = "area_name" } });

            _service = new GeoAreaFinder(_memoryAccessMock.Object);
        }

        [Fact]
        private void GetGeographicalArea_OnNullLatitude_ReturnsNull()
        {
            string result = _service.GetGeographicalArea(null, 11.11);

            Assert.Null(result);
        }

        [Fact]
        private void GetGeographicalArea_OnNullLongitude_ReturnsNull()
        {
            string result = _service.GetGeographicalArea(11.11, null);

            Assert.Null(result);
        }

        [Fact]
        private void GetGeographicalArea_OnCorrectCoordinates_ReturnsAreaName()
        {
            string expected = "area_name";

            string result = _service.GetGeographicalArea(5.0, 5.0);

            Assert.Equal(expected, result);
        }
    }
}
