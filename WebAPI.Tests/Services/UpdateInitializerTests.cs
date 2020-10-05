using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class UpdateInitializerTests
    {
        private readonly Mock<IUpdatingProgress> _progressMock;
        private readonly Mock<IDataAccessService> _dataServiceMock;
        private readonly Mock<IDataProcessor> _dataProcessorMock;
        private readonly UpdateInitializer _service;

        public UpdateInitializerTests()
        {
            _progressMock = new Mock<IUpdatingProgress>();
            _dataProcessorMock = new Mock<IDataProcessor>();
            _dataServiceMock = new Mock<IDataAccessService>();

            _progressMock.Setup(mock => mock.GetIsUpdatingStarted()).Returns(false);
            _dataServiceMock.Setup(mock => mock.GetVesselsToBeUpdated()).Returns(new List<VesselAisUpdateModel>());

            _service = new UpdateInitializer(_progressMock.Object, _dataServiceMock.Object, _dataProcessorMock.Object);
        }

        [Fact]
        private async Task StartUpdatesAsync_OnUpdatingStarted_NeverCallsMethods()
        {
            _progressMock.Setup(mock => mock.GetIsUpdatingStarted()).Returns(true);

            await _service.StartUpdatesAsync();

            _progressMock.Verify(mock => mock.SetUpdatingStarted(), Times.Never());
            _progressMock.Verify(mock => mock.SetUpdatingCompleted(), Times.Never());
            _dataServiceMock.Verify(mock => mock.SaveDatabaseQuantities(), Times.Never());
            _dataProcessorMock.Verify(mock => mock.UpdateListOfVesselsAsync(It.IsAny<List<VesselAisUpdateModel>>()), Times.Never());
            _dataServiceMock.Verify(mock => mock.GetVesselsToBeUpdated(), Times.Never());
            _progressMock.Verify(mock => mock.SetTotalResultsQuantities(It.IsAny<int>()), Times.Never());
        }

        [Fact]
        private async Task StartUpdatesAsync_OnUpdatingNotStarted_CallsAllMethods()
        {
            await _service.StartUpdatesAsync();

            _progressMock.Verify(mock => mock.SetUpdatingStarted(), Times.Once());
            _progressMock.Verify(mock => mock.SetUpdatingCompleted(), Times.Once());
            _dataServiceMock.Verify(mock => mock.SaveDatabaseQuantities(), Times.Once());
            _dataProcessorMock.Verify(mock => mock.UpdateListOfVesselsAsync(It.IsAny<List<VesselAisUpdateModel>>()), Times.Once());
            _dataServiceMock.Verify(mock => mock.GetVesselsToBeUpdated(), Times.Once());
            _progressMock.Verify(mock => mock.SetTotalResultsQuantities(It.IsAny<int>()), Times.Once());
        }
    }
}
