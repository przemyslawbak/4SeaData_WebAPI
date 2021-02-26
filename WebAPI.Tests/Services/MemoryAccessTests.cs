using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class MemoryAccessTests
    {
        private readonly Mock<IDataAccessService> _dataAccessMock;
        private MemoryAccess _service;
        private readonly List<AreaBboxModel> _expectedCollection;

        public MemoryAccessTests()
        {
            _expectedCollection = new List<AreaBboxModel>() { new AreaBboxModel() };
            _dataAccessMock = new Mock<IDataAccessService>();
        }

        public static IMemoryCache GetMemoryCacheCached(object expectedValue)
        {
            Mock<IMemoryCache> mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue)).Returns(true);
            return mockMemoryCache.Object;
        }

        public static IMemoryCache GetMemoryCacheNotCached(object expectedValue)
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return memoryCache;
        }

        [Fact]
        private void GetSeaAreas_OnCachedCollection_NeverCallsDataAccessService()
        {
            IMemoryCache memoryCache = GetMemoryCacheCached(_expectedCollection);
            _service = new MemoryAccess(memoryCache, _dataAccessMock.Object);

            IEnumerable<AreaBboxModel> result = _service.GetSeaAreas();

            _dataAccessMock.Verify(mock => mock.GetSeaAreas(), Times.Never());
            Assert.Single(result);
        }

        [Fact]
        private void GetSeaAreas_OnNotCachedCollection_CallsDataAccessService()
        {
            IMemoryCache memoryCache = GetMemoryCacheNotCached(_expectedCollection);
            _service = new MemoryAccess(memoryCache, _dataAccessMock.Object);
            _dataAccessMock.Setup(mock => mock.GetSeaAreas()).Returns(_expectedCollection);

            IEnumerable<AreaBboxModel> result = _service.GetSeaAreas();

            _dataAccessMock.Verify(mock => mock.GetSeaAreas(), Times.Once());
            Assert.Single(result);
        }
    }
}
