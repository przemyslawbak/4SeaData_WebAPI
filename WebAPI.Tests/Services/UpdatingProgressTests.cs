using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class UpdatingProgressTests
    {
        private readonly UpdatingProgress _service;

        public UpdatingProgressTests()
        {
            _service = new UpdatingProgress();
        }

        [Fact]
        private void SetLastUpdatedVessel_OnNullVesselString_DoesNotUpdateLastUpdatedVessel()
        {
            string expected = "some_vessel_data";

            _service.SetLastUpdatedVessel("some_vessel_data");
            _service.SetLastUpdatedVessel(null);

            StatusModel model = _service.GetProgressStatus();

            Assert.Equal(expected, model.LastUpdatedVessel);
        }

        [Fact]
        private void SetLastUpdatedVessel_OnCorrectVesselString_UpdatesLastUpdatedVessel()
        {
            string expected = "some_new_vessel_data";

            _service.SetLastUpdatedVessel("some_vessel_data");
            _service.SetLastUpdatedVessel("some_new_vessel_data");

            StatusModel model = _service.GetProgressStatus();

            Assert.Equal(expected, model.LastUpdatedVessel);
        }

        [Fact]
        private void UpdateMissingProperties_OnNullVesselModel_NotUpdatesCounters()
        {
            int expected = 0;
            _service.SetUpdatingStarted();

            _service.UpdateMissingProperties(null);
            StatusModel model = _service.GetProgressStatus();

            Assert.Equal(expected, model.MissingActivityTimes);
            Assert.Equal(expected, model.MissingAreas);
            Assert.Equal(expected, model.MissingCourses);
            Assert.Equal(expected, model.MissingDestinations);
            Assert.Equal(expected, model.MissingDraughts);
            Assert.Equal(expected, model.MissingEtas);
            Assert.Equal(expected, model.MissingLats);
            Assert.Equal(expected, model.MissingLongs);
            Assert.Equal(expected, model.MissingSpeeds);
            Assert.Equal(expected, model.MissingStatuses);
        }

        [Fact]
        private void UpdateMissingProperties_OnNotNullVesselModelWithNullProps_UpdatesCounters()
        {
            int expected = 1;
            VesselUpdateModel vesselUpdates = new VesselUpdateModel();
            _service.SetUpdatingStarted();

            _service.UpdateMissingProperties(vesselUpdates);
            StatusModel model = _service.GetProgressStatus();

            Assert.Equal(expected, model.MissingActivityTimes);
            Assert.Equal(expected, model.MissingAreas);
            Assert.Equal(expected, model.MissingCourses);
            Assert.Equal(expected, model.MissingDestinations);
            Assert.Equal(expected, model.MissingDraughts);
            Assert.Equal(expected, model.MissingEtas);
            Assert.Equal(expected, model.MissingLats);
            Assert.Equal(expected, model.MissingLongs);
            Assert.Equal(expected, model.MissingSpeeds);
            Assert.Equal(expected, model.MissingStatuses);
        }

        [Theory]
        [InlineData(1000, 800, 100, 900)]
        [InlineData(1000, 1000, 100, 1000)]
        [InlineData(1000, 900, 100, 1000)]
        private void GetCurrentUpdateStep_OnQuantities_ReturnsCorrectNextProgressValue(int totalQty, int counter, int step, int expected)
        {
            _service.SetTotalResultsQuantities(totalQty);

            int result = _service.GetCurrentUpdateStep(counter, step);

            Assert.Equal(expected, result);
        }
    }
}
