using Moq;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class NodeCreatorTests
    {
        private readonly Mock<IStringParser> _stringParserMock;
        private readonly NodeCreator _service;
        private readonly string _html;

        public NodeCreatorTests()
        {
            _stringParserMock = new Mock<IStringParser>();

            _html = "<html></html>";

            _service = new NodeCreator(_stringParserMock.Object);
        }

        [Fact]
        private void CreatePrepareAndVerifyRowNodeOuterHtml_OnNullRowNode_ReturnsNull()
        {
            _stringParserMock.Setup(mock => mock.GetXpath(It.IsAny<string>())).Returns("//body");

            string result = _service.CreatePrepareAndVerifyRowNodeOuterHtml(_html, "method_name");

            Assert.Null(result);
        }

        [Fact]
        private void CreatePrepareAndVerifyRowNodeOuterHtml_OnCorrectRowNode_ReturnsOuterHtml()
        {
            _stringParserMock.Setup(mock => mock.GetXpath(It.IsAny<string>())).Returns("//html");

            string result = _service.CreatePrepareAndVerifyRowNodeOuterHtml(_html, "method_name");

            Assert.Equal(_html, result);
        }

        [Theory]
        [InlineData("//body", true, null)]
        [InlineData("//html", false, null)]
        [InlineData("//html", true, "result")]
        private void CreatePrepareAndVerifyRowNodeString_OnVariousParameters_ReturnsCorrectResult(string html, bool ssTableRowCorrect, string expected)
        {
            _stringParserMock.Setup(mock => mock.GetXpath(It.IsAny<string>())).Returns(html);
            _stringParserMock.Setup(mock => mock.IsTableRowCorrect(It.IsAny<string>())).Returns(ssTableRowCorrect);
            _stringParserMock.Setup(mock => mock.SplitTableRow(It.IsAny<string>())).Returns("result");

            string result = _service.CreatePrepareAndVerifyRowNodeString(_html, "method_name");

            Assert.Equal(expected, result);
        }
    }
}
