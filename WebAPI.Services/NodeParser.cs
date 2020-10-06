using HtmlAgilityPack;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace WebAPI.Services
{
    public class NodeParser : INodeParser
    {
        private readonly IStringParser _stringParser;

        public NodeParser(IStringParser stringParser)
        {
            _stringParser = stringParser;
        }

        public double? ExtractSpeedFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyRowNodeString(html_document_2);
            string speed = _stringParser.GetTrimmedSpeed(node);
            return _stringParser.ParsedTrimmedNullableDouble(speed);
        }

        public double? ExtractCourseFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyRowNodeString(html_document_2);
            string course = _stringParser.GetTrimmedCourse(node);
            return _stringParser.ParsedTrimmedNullableDouble(course);
        }

        public string ExtractDestinationFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyRowNodeString(html_document_2);
            return _stringParser.GetUndashedDestination(node);
        }

        public double? ExtractDraughtFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyRowNodeString(html_document_2);
            string draught = _stringParser.GetTrimmedDraught(node);
            return _stringParser.ParsedTrimmedNullableDouble(draught);
        }

        public DateTime? ExtractEtaTimeFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyRowNodeString(html_document_2);
            string eta = _stringParser.GetTrimmedText(node);
            return _stringParser.ParsedTrimmedNullableDateTime(eta);
        }

        public double? ExtractLatFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyRowNodeString(html_document_2);
            string lat = _stringParser.GetTrimmedLatitude(node);
            return _stringParser.ParsedTrimmedNullableDouble(lat);
        }

        public double? ExtractLonFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyRowNodeString(html_document_2);
            string lon = _stringParser.GetTrimmedLongitude(node);
            return _stringParser.ParsedTrimmedNullableDouble(lon);
        }

        public DateTime? ExtractAisUpdateTimeFromHtml(string html_document_1, string html_document_2)
        {
            DateTime? time1 = ExtractAisUpdateTimeFromHtml1(html_document_1);
            DateTime? time2 = ExtractAisUpdateTimeFromHtml2(html_document_2);
            return CompareUpdateTimes(time1, time2);
        }

        public int ExtractMmsiFromHtml(string html_document_1)
        {
            if (CreateNodeCollection(html_document_1) != null)
            {
                foreach (HtmlNode row in CreateNodeCollection(html_document_1))
                {
                    if (_stringParser.IsContainingMmsi(row.InnerHtml))
                    {
                        return _stringParser.ParsedInt(row.ChildNodes[1].InnerText);
                    }
                }
            }

            return 0;
        }

        public string ExtractNaviStatusFromHtml(string html_document_1, DateTime? aISLatestActivity)
        {
            string node = CreatePrepareAndVerifyRowNodeOuterHtml(html_document_1);
            return CheckActivityTime(_stringParser.GetAisStatusTrimmed(node), aISLatestActivity);
        }

        private DateTime? ExtractAisUpdateTimeFromHtml2(string html_document_2)
        {
            string node = CreatePrepareAndVerifyRowNodeOuterHtml(html_document_2);
            string time = _stringParser.GetTrimmedTime1(node);
            return _stringParser.ParsedTrimmedNullableDateTime(time);
        }

        private DateTime? ExtractAisUpdateTimeFromHtml1(string html_document_1)
        {
            if (CreateNodeCollection(html_document_1) != null)
            {
                foreach (HtmlNode row in CreateNodeCollection(html_document_1))
                {
                    if (_stringParser.IsRowTimeRow(row.InnerHtml))
                    {
                        string time = _stringParser.GetTrimmedTime2(row.InnerHtml);
                        return _stringParser.ParsedTrimmedNullableDateTime(time);
                    }
                }
            }

            return null;
        }

        private DateTime? CompareUpdateTimes(DateTime? time1, DateTime? time2)
        {
            if (!time1.HasValue) return time2;
            if (!time2.HasValue) return time1;
            if (time1.HasValue && time2.HasValue && time1 > time2) return time1;

            return time2;
        }

        private HtmlDocument CreateNodeDocument(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }

        private HtmlNode CreateDocumentAndRowNode(string html, string callerName)
        {
            HtmlDocument doc = CreateNodeDocument(html);

            return doc.DocumentNode.SelectSingleNode(_stringParser.GetXpath(callerName));
        }

        private string CreatePrepareAndVerifyRowNodeOuterHtml(string html, [CallerMemberName] string callerName = "")
        {
            HtmlNode row = CreateDocumentAndRowNode(html, callerName);

            if (row != null)
            {
                return row.OuterHtml;
            }

            return null;
        }

        private string CreatePrepareAndVerifyRowNodeString(string html, [CallerMemberName] string callerName = "")
        {
            HtmlNode row = CreateDocumentAndRowNode(html, callerName);

            if (row != null)
            {
                if (_stringParser.IsTableRowCorrect(row.OuterHtml))
                {
                    return _stringParser.SplitRow(row.OuterHtml);
                }
            }

            return null;
        }

        private string CheckActivityTime(string text, DateTime? aISLatestActivity)
        {
            if (aISLatestActivity.HasValue)
            {
                if (aISLatestActivity.Value < DateTime.UtcNow.AddDays(-2))
                {
                    return "(out-of-date)";
                }
            }

            return text;
        }

        private HtmlNodeCollection CreateNodeCollection(string html_document_1, [CallerMemberName] string callerName = "")
        {
            HtmlDocument doc = CreateNodeDocument(html_document_1);

            return doc.DocumentNode.SelectNodes(_stringParser.GetXpath(callerName));
        }
    }
}
