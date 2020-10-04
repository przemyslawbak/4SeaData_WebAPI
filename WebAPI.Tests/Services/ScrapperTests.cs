using Microsoft.Extensions.Configuration;
using Moq;
using WebAPI.Services;

namespace WebAPI.Tests.Services
{
    public class ScrapperTests
    {
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<IHttpClientProvider> _httpClientMock;
        private readonly Mock<INodeParser> _nodeParserMock;
        private readonly Scrapper _service;

        public ScrapperTests()
        {
            _configMock = new Mock<IConfiguration>();
            _httpClientMock = new Mock<IHttpClientProvider>();
            _nodeParserMock = new Mock<INodeParser>();

            _service = new Scrapper(_configMock.Object, _httpClientMock.Object, _nodeParserMock.Object);
        }


    }
}
