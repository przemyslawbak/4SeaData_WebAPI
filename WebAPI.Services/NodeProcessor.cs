using HtmlAgilityPack;
using System;

namespace WebAPI.Services
{
    public class NodeProcessor : INodeProcessor
    {
        private readonly IStringParser _stringParser;
        private readonly INodeCreator _creator;

        public NodeProcessor(IStringParser stringParser, INodeCreator creator)
        {
            _stringParser = stringParser;
            _creator = creator;
        }

        public double? ExtractSpeedFromHtml(string html_document_2)
        {
            string node = _creator.CreatePrepareAndVerifyRowNodeString(html_document_2);
            string speed = _stringParser.GetTrimmedSpeed(node);
            return _stringParser.ParsedTrimmedNullableDouble(speed);
        }

        public double? ExtractCourseFromHtml(string html_document_2)
        {
            string node = _creator.CreatePrepareAndVerifyRowNodeString(html_document_2);
            string course = _stringParser.GetTrimmedCourse(node);
            return _stringParser.ParsedTrimmedNullableDouble(course);
        }

        public string ExtractDestinationFromHtml(string html_document_2)
        {
            string node = _creator.CreatePrepareAndVerifyRowNodeString(html_document_2);
            return _stringParser.GetUndashedTrimmedText(node);
        }

        public double? ExtractDraughtFromHtml(string html_document_2)
        {
            string node = _creator.CreatePrepareAndVerifyRowNodeString(html_document_2);
            string draught = _stringParser.GetTrimmedDraught(node);
            return _stringParser.ParsedTrimmedNullableDouble(draught);
        }

        public DateTime? ExtractEtaTimeFromHtml(string html_document_2)
        {
            string node = _creator.CreatePrepareAndVerifyRowNodeString(html_document_2);
            string eta = _stringParser.GetUndashedTrimmedText(node);
            return _stringParser.ParsedTrimmedNullableDateTime(eta);
        }

        public double? ExtractLatFromHtml(string html_document_2)
        {
            string node = _creator.CreatePrepareAndVerifyRowNodeString(html_document_2);
            string lat = _stringParser.GetTrimmedLatitude(node);
            return _stringParser.ParsedTrimmedNullableDouble(lat);
        }

        public double? ExtractLonFromHtml(string html_document_2)
        {
            string node = _creator.CreatePrepareAndVerifyRowNodeString(html_document_2);
            string lon = _stringParser.GetTrimmedLongitude(node);
            return _stringParser.ParsedTrimmedNullableDouble(lon);
        }

        public int ExtractMmsiFromHtml(string html_document_1)
        {
            if (_creator.CreateNodeCollection(html_document_1) != null)
            {
                foreach (HtmlNode row in _creator.CreateNodeCollection(html_document_1))
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
            string node = _creator.CreatePrepareAndVerifyRowNodeOuterHtml(html_document_1);
            return CheckActivityTime(_stringParser.GetAisStatusTrimmed(node), aISLatestActivity);
        }

        public DateTime? ExtractAisUpdateTimeFromHtml(string html_document_1, string html_document_2)//todo: unit test
        {
            DateTime? time1 = ExtractAisUpdateTimeFromHtml1(html_document_1);
            DateTime? time2 = ExtractAisUpdateTimeFromHtml2(html_document_2);
            return CompareUpdateTimes(time1, time2);
        }

        private DateTime? ExtractAisUpdateTimeFromHtml2(string html_document_2)
        {
            string node = _creator.CreatePrepareAndVerifyRowNodeOuterHtml(html_document_2);
            string time = _stringParser.GetTrimmedTime1(node);
            return _stringParser.ParsedTrimmedNullableDateTime(time);
        }

        private DateTime? ExtractAisUpdateTimeFromHtml1(string html_document_1)
        {
            if (_creator.CreateNodeCollection(html_document_1) != null)
            {
                foreach (HtmlNode row in _creator.CreateNodeCollection(html_document_1))
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
    }
}
