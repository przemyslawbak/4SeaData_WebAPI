using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using WebAPI.App.Controllers;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.Tests.Helpers;
using Xunit;

namespace WebAPI.Tests.Controllers
{
    public class UpdatesControllerTests
    {
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IUpdaterService> _updatesMock;
        private readonly UpdatesController _controller;
        private readonly StatusModel _statusModel;

        public UpdatesControllerTests()
        {
            _configMock = new Mock<IConfiguration>();
            _updatesMock = new Mock<IUpdaterService>();

            _statusModel = new StatusModel()
            {
                FailedResultsQuantity = 101,
                Finalizing = true,
                IsUpdatingInProgress = false,
                IsUpdatingPaused = true,
                LastCompletedTime = new DateTime(2000, 01, 02),
                LastError = "some_error",
                LastStartedTime = new DateTime(2000, 01, 03),
                LastUpdatedVessel = "some_vessel",
                MemoryMegabytesUsage = 152.66F,
                ReurnedVesselsInCurrent = 34623,
                TotalResultsQuantity = 190300,
                UpdatingDatabase = true,
                MissingStatuses = 2,
                MissingSpeeds = 1,
                MissingLongs = 0,
                MissingLats = 3,
                MissingEtas = 4,
                MissingActivityTimes = 5,
                MissingAreas = 6,
                MissingCourses = 7,
                MissingDestinations = 8,
                MissingDraughts = 9
            };

            _configMock.SetupGet(mock => mock[It.Is<string>(s => s == "Authentication:Key")]).Returns("test_security_key");
            _updatesMock.Setup(mock => mock.PauseUpdating()).Returns(true);
            _updatesMock.Setup(mock => mock.StopUpdatingAsync()).ReturnsAsync(true);
            _updatesMock.Setup(mock => mock.GetUpdatingStatus()).Returns(_statusModel);
            _updatesMock.Setup(mock => mock.UpdateSingleVesselAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            _controller = new UpdatesController(_configMock.Object, _updatesMock.Object);
        }

        [Fact]
        private void GetPauseDataUpdating_OnProperKeyAndPausedResult_Returns200()
        {
            string properKey = "test_security_key";

            IActionResult result = _controller.GetPauseDataUpdating(properKey);
            StatusCodeResult statusCode = result as StatusCodeResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, statusCode.StatusCode);
        }

        [Fact]
        private void GetPauseDataUpdating_OnWrongKey_Returns401()
        {
            string wrongKey = "_wrong_test_security_key";
            string expectedErrorsResult = "Unauthorized.";

            IActionResult result = _controller.GetPauseDataUpdating(wrongKey);
            ObjectResult objectResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
            Assert.Equal(expectedErrorsResult, objectResult.Value);
        }

        [Fact]
        private void GetPauseDataUpdating_OnPausingFailed_Returns417()
        {
            _updatesMock.Setup(mock => mock.PauseUpdating()).Returns(false);
            string properKey = "test_security_key";
            string expectedErrorsResult = "Updating is not started yet.";

            IActionResult result = _controller.GetPauseDataUpdating(properKey);
            ObjectResult objectResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status417ExpectationFailed, objectResult.StatusCode);
            Assert.Equal(expectedErrorsResult, objectResult.Value);
        }

        [Fact]
        private async Task GetStopDataUpdatingAsync_OnProperKeyAndStoppingResult_Returns200()
        {
            string properKey = "test_security_key";

            IActionResult result = await _controller.GetStopDataUpdatingAsync(properKey);
            StatusCodeResult statusCode = result as StatusCodeResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, statusCode.StatusCode);
        }

        [Fact]
        private async Task GetStopDataUpdatingAsync_OnWrongKey_Returns401()
        {
            string wrongKey = "_wrong_test_security_key";
            string expectedErrorsResult = "Unauthorized.";

            IActionResult result = await _controller.GetStopDataUpdatingAsync(wrongKey);
            ObjectResult objectResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
            Assert.Equal(expectedErrorsResult, objectResult.Value);
        }

        [Fact]
        private async Task GetStopDataUpdatingAsync_OnStoppingFailed_Returns417()
        {
            _updatesMock.Setup(mock => mock.StopUpdatingAsync()).ReturnsAsync(false);
            string properKey = "test_security_key";
            string expectedErrorsResult = "Failed to stop updating.";

            IActionResult result = await _controller.GetStopDataUpdatingAsync(properKey);
            ObjectResult objectResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status417ExpectationFailed, objectResult.StatusCode);
            Assert.Equal(expectedErrorsResult, objectResult.Value);
        }

        [Fact]
        private void GetUpdatingStatus_OnCorrectKey_ReturnsJsonResult()
        {
            string properKey = "test_security_key";

            IActionResult result = _controller.GetUpdatingStatus(properKey);
            JsonResult resultObject = result as JsonResult;
            dynamic resultData = new JsonResultDynamicWrapper(resultObject);

            Assert.NotNull(result);
            Assert.IsType<JsonResult>(result);
            Assert.Equal(101, resultData.FailedResultsQuantity);
            Assert.Equal(true, resultData.Finalizing);
            Assert.Equal(false, resultData.IsUpdatingInProgress);
            Assert.Equal(true, resultData.IsUpdatingPaused);
            Assert.Equal(true, resultData.UpdatingDatabase);
            Assert.Equal("some_error", resultData.LastError);
            Assert.Equal("some_vessel", resultData.LastUpdatedVessel);
            Assert.Equal(152.66F, resultData.MemoryMegabytesUsage);
            Assert.Equal(34623, resultData.ReurnedVesselsInCurrent);
            Assert.Equal(190300, resultData.TotalResultsQuantity);
            Assert.Equal(new DateTime(2000, 01, 03).ToString(), resultData.LastStartedTime.ToString());
            Assert.Equal(new DateTime(2000, 01, 02).ToString(), resultData.LastCompletedTime.ToString());
            Assert.Equal(2, resultData.MissingStatuses);
            Assert.Equal(1, resultData.MissingSpeeds);
            Assert.Equal(0, resultData.MissingLongs);
            Assert.Equal(3, resultData.MissingLats);
            Assert.Equal(4, resultData.MissingEtas);
            Assert.Equal(5, resultData.MissingActivityTimes);
            Assert.Equal(6, resultData.MissingAreas);
            Assert.Equal(7, resultData.MissingCourses);
            Assert.Equal(8, resultData.MissingDestinations);
            Assert.Equal(9, resultData.MissingDraughts);
        }

        [Fact]
        private void GetUpdatingStatus_OnWrongKey_Returns401()
        {
            string wrongKey = "_wrong_test_security_key";
            string expectedErrorsResult = "Unauthorized.";

            IActionResult result = _controller.GetUpdatingStatus(wrongKey);
            ObjectResult objectResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
            Assert.Equal(expectedErrorsResult, objectResult.Value);
        }

        [Fact]
        private async Task UpdateSingleVessel_OnWrongKey_Returns401()
        {
            string wrongKey = "_wrong_test_security_key";
            string expectedErrorsResult = "Unauthorized.";

            IActionResult result = await _controller.UpdateSingleVesselAsync(wrongKey, 319108200, 9390537, "basic");
            ObjectResult objectResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
            Assert.Equal(expectedErrorsResult, objectResult.Value);
        }

        [Fact]
        private async Task UpdateSingleVessel_OnReceivedNullVessel_Returns417()
        {
            _updatesMock.Setup(mock => mock.UpdateSingleVesselAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);
            string properKey = "test_security_key";
            string expectedErrorsResult = "Failed to update vessel.";

            IActionResult result = await _controller.UpdateSingleVesselAsync(properKey, 319108200, 9390537, "basic");
            ObjectResult objectResult = result as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status417ExpectationFailed, objectResult.StatusCode);
            Assert.Equal(expectedErrorsResult, objectResult.Value);
        }

        [Fact]
        private async Task UpdateSingleVessel_OnCorrectKeyAndReceivedVessel_Returns200()
        {
            string properKey = "test_security_key";

            IActionResult result = await _controller.UpdateSingleVesselAsync(properKey, 319108200, 9390537, "basic");
            StatusCodeResult statusCode = result as StatusCodeResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, statusCode.StatusCode);
        }
    }
}
