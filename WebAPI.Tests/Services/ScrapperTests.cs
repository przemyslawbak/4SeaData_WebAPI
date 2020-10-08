using Microsoft.Extensions.Configuration;
using Moq;
using System;
using WebAPI.Models;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class ScrapperTests
    {
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IHttpClientProvider> _httpClientMock;
        private readonly Mock<INodeProcessor> _nodeParserMock;
        private readonly Mock<IGeoAreaFinder> _areaFinder;
        private readonly Scrapper _service;

        public ScrapperTests()
        {
            _configMock = new Mock<IConfiguration>();
            _httpClientMock = new Mock<IHttpClientProvider>();
            _nodeParserMock = new Mock<INodeProcessor>();
            _areaFinder = new Mock<IGeoAreaFinder>();

            _httpClientMock.Setup(mock => mock.GetHtmlDocument(It.IsAny<string>())).Returns("some_html_document");
            _nodeParserMock.Setup(mock => mock.ExtractMmsiFromHtml(It.IsAny<string>())).Returns(11111111);
            _nodeParserMock.Setup(mock => mock.ExtractLatFromHtml(It.IsAny<string>())).Returns(11.111);
            _nodeParserMock.Setup(mock => mock.ExtractLonFromHtml(It.IsAny<string>())).Returns(12.112);
            _nodeParserMock.Setup(mock => mock.ExtractAisUpdateTimeFromHtml(It.IsAny<string>(), It.IsAny<string>())).Returns(new DateTime(2020, 01, 01));

            _service = new Scrapper(_configMock.Object, _httpClientMock.Object, _nodeParserMock.Object, _areaFinder.Object);
        }

        [Fact]
        private void ScrapSingleVessel_OnMmsiEqualZero_CallsExtractMmsiFromHtmlOnce()
        {
            VesselUpdateModel model = _service.ScrapSingleVessel(0, 11111112);

            _nodeParserMock.Verify(mock => mock.ExtractMmsiFromHtml(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        private void ScrapSingleVessel_OnMmsiDifferentThanZero_NeverCallsExtractMmsiFromHtml()
        {
            VesselUpdateModel model = _service.ScrapSingleVessel(11111111, 11111112);

            _nodeParserMock.Verify(mock => mock.ExtractMmsiFromHtml(It.IsAny<string>()), Times.Never());
        }
    }
}
