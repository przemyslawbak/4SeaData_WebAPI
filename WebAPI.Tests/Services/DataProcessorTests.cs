using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class DataProcessorTests
    {
        private readonly Mock<IUpdatingProgress> _progressMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IUpdatedVesselFactory> _updatesMock;
        private readonly Mock<IStringParser> _stringParser;
        private readonly Mock<IDataAccessService> _dataServMock;
        private readonly Mock<IExceptionProcessor> _exceptionsMock;
        private readonly DataProcessor _service;
        private readonly List<VesselUpdateModel> _returnedVessels;
        private readonly VesselUpdateModel _returnedSingleVessel;
        private readonly string _exMethodName;

        public DataProcessorTests()
        {
            _progressMock = new Mock<IUpdatingProgress>();
            _configMock = new Mock<IConfiguration>();
            _updatesMock = new Mock<IUpdatedVesselFactory>();
            _stringParser = new Mock<IStringParser>();
            _dataServMock = new Mock<IDataAccessService>();
            _exceptionsMock = new Mock<IExceptionProcessor>();

            _exMethodName = "method_name";
            _returnedSingleVessel = new VesselUpdateModel()
            {
                Course = 0.1,
                AISLatestActivity = null,
                AISStatus = "some_status",
                Destination = "some_dest",
                ETA = null,
                GeographicalArea = "some_area",
                Draught = 1.1,
                IMO = 11111111,
                Lat = 1.11,
                Lon = 1.12,
                MMSI = 11111112,
                Speed = 1.3
            };
            _returnedVessels = new List<VesselUpdateModel>()
            {
                _returnedSingleVessel
            };

            _exceptionsMock.Setup(mock => mock.GetMethodNameThrowingException(It.IsAny<Exception>())).Returns(_exMethodName);
            _configMock.Setup(mock => mock.GetSection("Iteration:ParalelizmDegree")).Returns(new Mock<IConfigurationSection>().Object);
            _configMock.Setup(mock => mock.GetSection("Iteration:Step")).Returns(new Mock<IConfigurationSection>().Object);
            _progressMock.Setup(mock => mock.GetTotalResultsQuantity()).Returns(1);
            _progressMock.Setup(mock => mock.GetCurrentUpdateStep(It.IsAny<int>(), It.IsAny<int>())).Returns(1);

            _service = new DataProcessor
                (_progressMock.Object,
                _configMock.Object,
                _updatesMock.Object,
                _stringParser.Object,
                _dataServMock.Object,
                _exceptionsMock.Object);
        }

        [Fact]
        private async Task UpdateSingleVesselAsync_OnReceivedVessel_ReturnsTrueAndCallsSaveUpdatedVesselOnce()
        {
            _updatesMock.Setup(mock => mock.GetVesselUpdatesAsync(It.IsAny<VesselAisUpdateModel>(), It.IsAny<CancellationToken>(), It.IsAny<SemaphoreSlim>()))
                .ReturnsAsync(_returnedSingleVessel);

            bool result = await _service.UpdateSingleVesselAsync(11111111, 11111112, "basic");

            Assert.True(result);
            _dataServMock.Verify(mock => mock.SaveUpdatedVessels(_returnedVessels), Times.Once);
        }

        [Fact]
        private async Task UpdateSingleVesselAsync_OnReceivedNull_ReturnsFalseAndNeverCallsSaveUpdatedVessel()
        {
            _updatesMock.Setup(mock => mock.GetVesselUpdatesAsync(It.IsAny<VesselAisUpdateModel>(), It.IsAny<CancellationToken>(), It.IsAny<SemaphoreSlim>()))
                .ReturnsAsync((VesselUpdateModel)null);

            bool result = await _service.UpdateSingleVesselAsync(11111111, 11111112, "basic");

            Assert.False(result);
            _dataServMock.Verify(mock => mock.SaveUpdatedVessels(_returnedVessels), Times.Never);
        }

        [Fact]
        private async Task ProcessNextStepAsync_OnCatchedException_CallsSetLastErrorOnce()
        {
            string ex = "some_exception";
            _updatesMock.Setup(mock => mock.GetVesselUpdatesAsync(It.IsAny<VesselAisUpdateModel>(), It.IsAny<CancellationToken>(), It.IsAny<SemaphoreSlim>()))
                .Throws(new Exception(ex));

            await _service.UpdateListOfVesselsAsync(new List<VesselAisUpdateModel>() { new VesselAisUpdateModel() });

            
            _progressMock.Verify(mock => mock.SetLastError(ex + " from: " + _exMethodName), Times.Once());
        }

    }
}
