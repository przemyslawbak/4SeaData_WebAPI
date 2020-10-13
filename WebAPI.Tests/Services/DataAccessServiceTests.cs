using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class DataAccessServiceTests
    {
        private readonly Mock<IServiceScopeFactory> _scopeFactoryMock;
        private readonly Mock<IUpdatingProgress> _progressMock;
        private readonly Mock<ISqlQueryBuilder> _sqlBuilderMock;
        private readonly DataAccessService _service;

        public DataAccessServiceTests()
        {
            _scopeFactoryMock = new Mock<IServiceScopeFactory>();
            _sqlBuilderMock = new Mock<ISqlQueryBuilder>();
            _progressMock = new Mock<IUpdatingProgress>();

            _service = new DataAccessService(_scopeFactoryMock.Object, _sqlBuilderMock.Object, _progressMock.Object);
        }

        [Fact]
        private void SaveUpdatedVessels_OnEmptyListReceived_NeverCallsCreateAndSendUpdatesQuery()
        {
            _service.SaveUpdatedVessels(new List<VesselUpdateModel>());

            _sqlBuilderMock.Verify(mock => mock.CreateAndSendUpdatesQuery(It.IsAny<List<VesselUpdateModel>>()), Times.Never);
        }

        [Fact]
        private void SaveUpdatedVessels_OnPopulatedListReceived_CallsCreateAndSendUpdatesQuery()
        {
            _service.SaveUpdatedVessels(new List<VesselUpdateModel>() { new VesselUpdateModel() });

            _sqlBuilderMock.Verify(mock => mock.CreateAndSendUpdatesQuery(It.IsAny<List<VesselUpdateModel>>()), Times.Once);
        }
    }
}
