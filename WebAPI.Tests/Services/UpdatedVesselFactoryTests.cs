using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class UpdatedVesselFactoryTests
    {
        private readonly Mock<IUpdatingProgress> _progressMock;
        private readonly Mock<IScrapper> _scrapperMock;
        private readonly Mock<IExceptionProcessor> _exceptionProcessorMock;
        private readonly UpdatedVesselFactory _service;

        private readonly int _correctImo;
        private readonly string _exMethodName;

        public UpdatedVesselFactoryTests()
        {
            _progressMock = new Mock<IUpdatingProgress>();
            _scrapperMock = new Mock<IScrapper>();
            _exceptionProcessorMock = new Mock<IExceptionProcessor>();

            _exMethodName = "method_name";
            _correctImo = 9482469;
            VesselUpdateModel vslModel = new VesselUpdateModel()
            {
                AISLatestActivity = new DateTime(2020, 10, 02),
                AISStatus = "some_status",
                ETA = new DateTime(2020, 10, 03),
                GeographicalArea = "some_sea",
                Course = 200.3,
                Speed = 8.4,
                Destination = "some_port",
                Draught = 10.3,
                IMO = _correctImo,
                MMSI = 312619000,
                Lat = 41.842,
                Lon = 3.7294
            };

            _scrapperMock.Setup(mock => mock.ScrapSingleVessel(It.IsAny<int>(), It.IsAny<int>())).Returns(vslModel);
            _progressMock.Setup(mock => mock.GetIsUpdatingDatabase()).Returns(false);
            _progressMock.Setup(mock => mock.GetIsUpdatingPaused()).Returns(false);
            _exceptionProcessorMock.Setup(mock => mock.GetMethodNameThrowingException(It.IsAny<Exception>())).Returns(_exMethodName);

            _service = new UpdatedVesselFactory(_scrapperMock.Object, _progressMock.Object, _exceptionProcessorMock.Object);
        }

        [Fact]
        private void GetVesselUpdatesAsync_OnMmsiEqualZero_ReturnsNullAndCallsMethodsInFinallyOnce()
        {
            VesselAisUpdateModel updateModel = new VesselAisUpdateModel() { Mmsi = 0 };

            Task<VesselUpdateModel> result = _service.GetVesselUpdatesAsync(updateModel);

            Assert.Null(result.Result);
            _progressMock.Verify(mock => mock.GetIsUpdatingDatabase(), Times.Once());
            _progressMock.Verify(mock => mock.GetIsUpdatingPaused(), Times.Once());
        }

        [Fact]
        private void GetVesselUpdatesAsync_OnSpeedNull_ReturnsNullAndCallsMethodsInFinallyOnce()
        {
            VesselAisUpdateModel updateModel = new VesselAisUpdateModel() { Speed = null };

            Task<VesselUpdateModel> result = _service.GetVesselUpdatesAsync(updateModel);

            Assert.Null(result.Result);
            _progressMock.Verify(mock => mock.GetIsUpdatingDatabase(), Times.Once());
            _progressMock.Verify(mock => mock.GetIsUpdatingPaused(), Times.Once());
        }

        [Fact]
        private void GetVesselUpdatesAsync_OnCorrectVesselModel_ReturnsVesselUpdateModel()
        {
            VesselAisUpdateModel updateModel = new VesselAisUpdateModel() { Mmsi = 12345678, Speed = 0.1, Imo = _correctImo };

            Task<VesselUpdateModel> result = _service.GetVesselUpdatesAsync(updateModel);

            Assert.NotNull(result.Result);
            Assert.Equal(new DateTime(2020, 10, 02), result.Result.AISLatestActivity);
            Assert.Equal("some_status", result.Result.AISStatus);
            Assert.Equal(new DateTime(2020, 10, 03), result.Result.ETA);
            Assert.Equal(200.3, result.Result.Course);
            Assert.Equal(8.4, result.Result.Speed);
            Assert.Equal("some_port", result.Result.Destination);
            Assert.Equal(10.3, result.Result.Draught);
            Assert.Equal(9482469, result.Result.IMO);
            Assert.Equal(312619000, result.Result.MMSI);
            Assert.Equal(3.7294, result.Result.Lon);
            Assert.Equal(41.842, result.Result.Lat);
            _progressMock.Verify(mock => mock.GetIsUpdatingDatabase(), Times.Exactly(2));
            _progressMock.Verify(mock => mock.GetIsUpdatingPaused(), Times.Exactly(2));
        }

        [Fact]
        private void GetVesselUpdatesAsync_OnExceptionThrownByScrapSingleVessel_CallsMethodsInCatchOnce()
        {
            string ex = "some_exception";
            _scrapperMock.Setup(mock => mock.ScrapSingleVessel(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception(ex));
            VesselAisUpdateModel updateModel = new VesselAisUpdateModel() { Mmsi = 12345678, Speed = 0.1, Imo = _correctImo };

            Task<VesselUpdateModel> result = _service.GetVesselUpdatesAsync(updateModel);

            Assert.Null(result.Result);
            _progressMock.Verify(mock => mock.GetIsUpdatingDatabase(), Times.Once());
            _progressMock.Verify(mock => mock.GetIsUpdatingPaused(), Times.Once());
            _progressMock.Verify(mock => mock.AddFailedRequest(), Times.Once());
            _progressMock.Verify(mock => mock.SetLastError(ex + " from: " + _exMethodName), Times.Once());
        }

        [Fact]
        private void GetVesselUpdatesAsync_OnReceivedResultWithDifferentImo_ThrowsException()
        {
            int _incorrectImo = _correctImo - 1;
            string ex = "Received vessel imo differs from the one passed.";
            VesselAisUpdateModel updateModel = new VesselAisUpdateModel() { Imo = 9482469, Mmsi = 12345678, Speed = 0.1 };
            VesselUpdateModel returnedModel = new VesselUpdateModel() { IMO = _incorrectImo };
            _scrapperMock.Setup(mock => mock.ScrapSingleVessel(It.IsAny<int>(), It.IsAny<int>())).Returns(returnedModel);

            Task<VesselUpdateModel> result = _service.GetVesselUpdatesAsync(updateModel);

            Assert.Null(result.Result);
            _progressMock.Verify(mock => mock.GetIsUpdatingDatabase(), Times.Once());
            _progressMock.Verify(mock => mock.GetIsUpdatingPaused(), Times.Once());
            _progressMock.Verify(mock => mock.AddFailedRequest(), Times.Once());
            _progressMock.Verify(mock => mock.SetLastError(ex + " from: " + _exMethodName), Times.Once());
        }
    }
}
