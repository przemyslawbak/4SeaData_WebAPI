using HtmlAgilityPack;
using Moq;
using System;
using WebAPI.Services;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class NodeProcessorTests
    {
        private readonly Mock<IStringParser> _stringParserMock;
        private readonly Mock<INodeCreator> _creatorMock;
        private readonly NodeProcessor _service;

        private readonly HtmlNodeCollection _returnedNodeCollection;
        private readonly int _returnedMmsi;

        public NodeProcessorTests()
        {
            _stringParserMock = new Mock<IStringParser>();
            _creatorMock = new Mock<INodeCreator>();

            _returnedMmsi = 11111111;
            HtmlNode childNode1 = HtmlNode.CreateNode("<h2> This is h2 heading1</h2>");
            HtmlNode childNode2 = HtmlNode.CreateNode("<h2> This is h2 heading2</h2>");
            HtmlNode node = new HtmlNode(new HtmlNodeType(), new HtmlDocument(), 0)
            {
                InnerHtml = "some_html"
            };
            node.AppendChild(childNode1);
            node.AppendChild(childNode2);
            _returnedNodeCollection = new HtmlNodeCollection(new HtmlNode(new HtmlNodeType(), new HtmlDocument(), 0));
            _returnedNodeCollection.Append(node);

            _creatorMock.Setup(mock => mock.CreateNodeCollection(It.IsAny<string>(), It.IsAny<string>())).Returns(_returnedNodeCollection);
            _creatorMock.Setup(mock => mock.CreatePrepareAndVerifyRowNodeOuterHtml(It.IsAny<string>(), It.IsAny<string>())).Returns("some_node_string");
            _stringParserMock.Setup(mock => mock.IsContainingMmsi(It.IsAny<string>())).Returns(true);
            _stringParserMock.Setup(mock => mock.ParsedInt(It.IsAny<string>())).Returns(_returnedMmsi);
            _stringParserMock.Setup(mock => mock.GetAisStatusTrimmed(It.IsAny<string>())).Returns("some_status_string");
            _stringParserMock.Setup(mock => mock.GetTrimmedTime1(It.IsAny<string>())).Returns("some_date_time_string1");
            _stringParserMock.Setup(mock => mock.GetTrimmedTime2(It.IsAny<string>())).Returns("some_date_time_string2");
            _stringParserMock.Setup(mock => mock.IsRowTimeRow(It.IsAny<string>())).Returns(true);
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.IsAny<string>())).Returns(new DateTime(2020, 10, 08));

            _service = new NodeProcessor(_stringParserMock.Object, _creatorMock.Object);
        }

        [Fact]
        private void ExtractMmsiFromHtml_OnReceivedNullCollection_returnsZeroValue()
        {
            _creatorMock.Setup(mock => mock.CreateNodeCollection(It.IsAny<string>(), It.IsAny<string>())).Returns((HtmlNodeCollection)null);

            int result = _service.ExtractMmsiFromHtml("some_html");

            Assert.Equal(0, result);
            _creatorMock.Verify(mock => mock.CreateNodeCollection(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        private void ExtractMmsiFromHtml_OnReceivedCollection_returnsCorrectValue()
        {
            int expected = 11111111;

            int result = _service.ExtractMmsiFromHtml("some_html");

            Assert.Equal(expected, result);
            _creatorMock.Verify(mock => mock.CreateNodeCollection(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            _stringParserMock.Verify(mock => mock.IsContainingMmsi(It.IsAny<string>()), Times.Once);
            _stringParserMock.Verify(mock => mock.ParsedInt(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        private void ExtractNaviStatusFromHtml_OnExpiredActivityTime_ReturnsOutOfDate()
        {
            string expected = "(out-of-date)";

            string result = _service.ExtractNaviStatusFromHtml("some_html", DateTime.UtcNow.AddDays(-3));

            Assert.Equal(expected, result);
        }

        [Fact]
        private void ExtractNaviStatusFromHtml_OnNotExpiredYetActivityTime_ReturnsStatus()
        {
            string expected = "some_status_string";

            string result = _service.ExtractNaviStatusFromHtml("some_html", DateTime.UtcNow.AddDays(-1));

            Assert.Equal(expected, result);
        }

        [Fact]
        private void ExtractAisUpdateTimeFromHtml_OnReceivedNullCollection_NeverCallsIsRowTimeRowAndGetTrimmedTime2()
        {
            _creatorMock.Setup(mock => mock.CreateNodeCollection(It.IsAny<string>(), It.IsAny<string>())).Returns((HtmlNodeCollection)null);

            DateTime? result = _service.ExtractAisUpdateTimeFromHtml("html1", "html2");

            _stringParserMock.Verify(mock => mock.IsRowTimeRow(It.IsAny<string>()), Times.Never());
            _stringParserMock.Verify(mock => mock.GetTrimmedTime2(It.IsAny<string>()), Times.Never());
            Assert.Equal(new DateTime(2020, 10, 08), result);
        }

        [Fact]
        private void ExtractAisUpdateTimeFromHtml_OnReceivedFalseFromIsRowTimeRow_NeverCallsGetTrimmedTime2()
        {
            _stringParserMock.Setup(mock => mock.IsRowTimeRow(It.IsAny<string>())).Returns(false);

            DateTime? result = _service.ExtractAisUpdateTimeFromHtml("html1", "html2");

            _stringParserMock.Verify(mock => mock.GetTrimmedTime2(It.IsAny<string>()), Times.Never());
            Assert.Equal(new DateTime(2020, 10, 08), result);
        }

        [Fact]
        private void ExtractAisUpdateTimeFromHtml_OnTime1Null_ReturnsTime2()
        {
            DateTime? expected = new DateTime(2020, 10, 09);
            string time1 = null;
            string time2 = "2020/10/09";
            _stringParserMock.Setup(mock => mock.GetTrimmedTime1(It.IsAny<string>())).Returns(time1);
            _stringParserMock.Setup(mock => mock.GetTrimmedTime2(It.IsAny<string>())).Returns(time2);
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.Is<string>(d => d == time1))).Returns((DateTime?)null);
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.Is<string>(d => d == time2))).Returns(new DateTime(2020, 10, 09));

            DateTime? result = _service.ExtractAisUpdateTimeFromHtml("html1", "html2");

            Assert.Equal(expected, result);
        }

        [Fact]
        private void ExtractAisUpdateTimeFromHtml_OnTime2Null_ReturnsTime1()
        {
            DateTime? expected = new DateTime(2020, 10, 09);
            string time2 = null;
            string time1 = "2020/10/09";
            _stringParserMock.Setup(mock => mock.GetTrimmedTime1(It.IsAny<string>())).Returns(time1);
            _stringParserMock.Setup(mock => mock.GetTrimmedTime2(It.IsAny<string>())).Returns(time2);
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.Is<string>(d => d == time2))).Returns((DateTime?)null);
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.Is<string>(d => d == time1))).Returns(new DateTime(2020, 10, 09));

            DateTime? result = _service.ExtractAisUpdateTimeFromHtml("html1", "html2");

            Assert.Equal(expected, result);
        }

        [Fact]
        private void ExtractAisUpdateTimeFromHtml_OnTime2OlderThanTime1_ReturnsTime2()
        {
            DateTime? expected = new DateTime(2020, 10, 09);
            string time2 = "2020/10/08";
            string time1 = "2020/10/09";
            _stringParserMock.Setup(mock => mock.GetTrimmedTime1(It.IsAny<string>())).Returns(time1);
            _stringParserMock.Setup(mock => mock.GetTrimmedTime2(It.IsAny<string>())).Returns(time2);
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.Is<string>(d => d == time2))).Returns(new DateTime(2020, 10, 08));
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.Is<string>(d => d == time1))).Returns(new DateTime(2020, 10, 09));

            DateTime? result = _service.ExtractAisUpdateTimeFromHtml("html1", "html2");

            Assert.Equal(new DateTime(2020, 10, 09), result);
        }

        [Fact]
        private void ExtractAisUpdateTimeFromHtml_OnTime1OlderThanTime2_ReturnsTime1()
        {
            DateTime? expected = new DateTime(2020, 10, 09);
            string time2 = "2020/10/09";
            string time1 = "2020/10/08";
            _stringParserMock.Setup(mock => mock.GetTrimmedTime1(It.IsAny<string>())).Returns(time1);
            _stringParserMock.Setup(mock => mock.GetTrimmedTime2(It.IsAny<string>())).Returns(time2);
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.Is<string>(d => d == time2))).Returns(new DateTime(2020, 10, 09));
            _stringParserMock.Setup(mock => mock.ParsedTrimmedNullableDateTime(It.Is<string>(d => d == time1))).Returns(new DateTime(2020, 10, 08));

            DateTime? result = _service.ExtractAisUpdateTimeFromHtml("html1", "html2");

            Assert.Equal(new DateTime(2020, 10, 09), result);
        }
    }
}
