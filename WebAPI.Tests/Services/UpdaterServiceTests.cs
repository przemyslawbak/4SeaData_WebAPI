using Moq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class UpdaterServiceTests
    {
        private readonly Mock<ITimeHostedUpdater> _hostedUpdaterMock;
        private readonly Mock<IProgressService> _progressMock;
        private readonly UpdaterService _service;

        public UpdaterServiceTests()
        {
            _hostedUpdaterMock = new Mock<ITimeHostedUpdater>();
            _progressMock = new Mock<IProgressService>();

            _progressMock.Setup(mock => mock.GetNewCalnellationToken()).Returns(new CancellationToken());

            _service = new UpdaterService(_hostedUpdaterMock.Object, _progressMock.Object);
        }

        [Theory]
        [InlineData(true, true, true, 1, 0)]
        [InlineData(true, false, true, 0, 1)]
        [InlineData(false, true, false, 0, 0)]
        [InlineData(false, false, false, 0, 0)]
        private void PauseUpdating_OnUpdaterPausedAndStartedValues_ReturnsCorrectBoolAndCallsCorrectMethods(bool isUpdatingStarted, bool isUpdatingPaused, bool expected, int timesCalledUpdatingUnpaused, int timesCalledUpdatingPaused)
        {
            _progressMock.Setup(mock => mock.GetIsUpdatingPaused()).Returns(isUpdatingPaused);
            _progressMock.Setup(mock => mock.GetIsUpdatingStarted()).Returns(isUpdatingStarted);

            bool result = _service.PauseUpdating();

            Assert.Equal(expected, result);
            _progressMock.Verify(mock => mock.SetUpdatingPaused(), Times.Exactly(timesCalledUpdatingPaused));
            _progressMock.Verify(mock => mock.SetUpdatingUnpaused(), Times.Exactly(timesCalledUpdatingUnpaused));
        }

        [Theory]
        [InlineData(true, false, 0, 0)]
        [InlineData(false, true, 1, 1)]
        private async Task StartUpdatingAsync_OnUpdaterdStartedValue_ReturnsCorrectBool(bool isUpdatingStarted, bool expected, int timesCalledStartAsync, int timesGetCalnellationToken)
        {
            _progressMock.Setup(mock => mock.GetIsUpdatingStarted()).Returns(isUpdatingStarted);

            bool result = await _service.StartUpdatingAsync();

            Assert.Equal(expected, result);
            _hostedUpdaterMock.Verify(mock => mock.StartAsync(It.IsAny<CancellationToken>()), Times.Exactly(timesCalledStartAsync));
            _progressMock.Verify(mock => mock.GetNewCalnellationToken(), Times.Exactly(timesGetCalnellationToken));
        }

        [Theory]
        [InlineData(true, true, 1, 1)]
        [InlineData(false, false, 0, 0)]
        private async Task StopUpdatingAsync_OnUpdaterdStartedValue_ReturnsCorrectBool(bool isUpdatingStarted, bool expected, int timesCalledStartAsync, int timesGetTokenSource)
        {
            _progressMock.Setup(mock => mock.GetIsUpdatingStarted()).Returns(isUpdatingStarted);

            bool result = await _service.StopUpdatingAsync();

            Assert.Equal(expected, result);
            _hostedUpdaterMock.Verify(mock => mock.StopAsync(It.IsAny<CancellationTokenSource>()), Times.Exactly(timesCalledStartAsync));
            _progressMock.Verify(mock => mock.GetTokenSource(), Times.Exactly(timesGetTokenSource));
        }
    }
}
