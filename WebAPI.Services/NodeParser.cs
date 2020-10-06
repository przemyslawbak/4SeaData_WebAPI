using HtmlAgilityPack;
using System;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            string node = CreatePrepareAndVerifyDocument(html_document_2);
            string speed = _stringParser.GetTrimmedSpeed(node);
            return _stringParser.ParsedTrimmedNullableDouble(speed);
        }

        public double? ExtractCourseFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyDocument(html_document_2);
            string course = _stringParser.GetTrimmedCourse(node);
            return _stringParser.ParsedTrimmedNullableDouble(course);
        }

        public string ExtractDestinationFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyDocument(html_document_2);
            return _stringParser.GetUndashedDestination(node);
        }

        public double? ExtractDraughtFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyDocument(html_document_2);
            string draught = _stringParser.GetTrimmedDraught(node);
            return _stringParser.ParsedTrimmedNullableDouble(draught);
        }

        public DateTime? ExtractEtaTimeFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyDocument(html_document_2);
            string eta = _stringParser.GetTrimmedText(node);
            return _stringParser.ParsedTrimmedNullableDateTime(eta);
        }

        public double? ExtractLatFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyDocument(html_document_2);
            string lat = _stringParser.GetTrimmedLatitude(node);
            return _stringParser.ParsedTrimmedNullableDouble(lat);
        }

        public double? ExtractLonFromHtml(string html_document_2)
        {
            string node = CreatePrepareAndVerifyDocument(html_document_2);
            string lon = _stringParser.GetTrimmedLongitude(node);
            return _stringParser.ParsedTrimmedNullableDouble(lon);
        }




        public int ExtractMmsiFromHtml(string html_document_1, int imo)
        {
            HtmlDocument doc = CreateNodeDocument(html_document_1);

            HtmlNodeCollection rows = doc.DocumentNode.SelectNodes(_stringParser.GetXpath(MethodBase.GetCurrentMethod().Name));

            if (rows != null)
            {
                foreach (HtmlNode row in rows)
                {
                    if (row.InnerHtml.ToString().Contains("MMSI:"))
                    {
                        string mmsi = row.ChildNodes[1].InnerText;

                        return _stringParser.ParsedInt(mmsi);
                    }
                }
            }

            return 0;
        }

        public string ExtractNaviStatusFromHtml(string html_document_1, DateTime? aISLatestActivity)
        {
            HtmlDocument doc = CreateNodeDocument(html_document_1);

            HtmlNode naviStatus = doc.DocumentNode.SelectSingleNode(_stringParser.GetXpath(MethodBase.GetCurrentMethod().Name));
            if (naviStatus != null && naviStatus.OuterHtml.ToString().Contains("Navigational status:"))
            {
                string aISStatus = naviStatus.OuterHtml.ToString().Split(new string[] { "Navigational status:</div><div class=" }, StringSplitOptions.None)[1].Split('>')[1].Split('<')[0].Trim();
                aISStatus = WebUtility.HtmlDecode(aISStatus);
            }

            if (aISLatestActivity.HasValue)
            {
                if (aISLatestActivity.Value < DateTime.UtcNow.AddDays(-2))
                {
                    return "(out-of-date)";
                }
            }

            return null;
        }

        public DateTime? ExtractAisUpdateTimeFromHtml(string html_document_1, string html_document_2)
        {
            DateTime? time1 = null;
            DateTime? time2 = null;
            HtmlDocument doc = CreateNodeDocument(html_document_1);

            HtmlNodeCollection rows = doc.DocumentNode.SelectNodes(_stringParser.GetXpath(MethodBase.GetCurrentMethod().Name) + "1");
            if (rows != null)
            {
                foreach (HtmlNode row in rows)
                {
                    if (row.InnerHtml.ToString().Contains("Last seen:"))
                    {
                        if (row.InnerHtml.ToString().Contains("<span>") && row.InnerHtml.ToString().Contains("<"))
                        {
                            string time = row.InnerHtml.ToString()
                            .Split(new string[] { "<span>" }, StringSplitOptions.None)[1]
                            .Split('<')[0];

                            if (DateTime.TryParse(time, out DateTime date))
                            {
                                time1 = date;
                            }
                        }
                    }
                }
            }

            doc = CreateNodeDocument(html_document_2);
            HtmlNode feedTime = doc.DocumentNode.SelectSingleNode(_stringParser.GetXpath(MethodBase.GetCurrentMethod().Name) + "2");
            if (feedTime != null && feedTime.OuterHtml.ToString().Contains(" UTC") && feedTime.OuterHtml.ToString().Contains("<td class=\"v3 tooltip expand\" data-title=\""))
            {
                string time = feedTime.OuterHtml.ToString().Split(new string[] { "<td class=\"v3 tooltip expand\" data-title=\"" }, StringSplitOptions.None)[1]
                    .Split(new string[] { " UTC" }, StringSplitOptions.None)[0].Trim();

                if (DateTime.TryParse(time, out DateTime date))
                {
                    time2 = date;
                }
            }

            if (time1.HasValue)
            {
                if (time2.HasValue)
                {
                    if (time1 > time2)
                    {
                        return time1;
                    }
                    else
                    {
                        return time2;
                    }
                }
                else
                {
                    return time1;
                }
            }
            else
            {
                return time2;
            }
        }

        private HtmlDocument CreateNodeDocument(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }

        private string CreatePrepareAndVerifyDocument(string html, [CallerMemberName] string callerName = "")
        {
            HtmlNode row = CreateDocumentAndRowNode(html, callerName);

            if (_stringParser.IsTableRowCorrect(row.OuterHtml.ToString()))
            {
                return _stringParser.SplitRow(row.OuterHtml.ToString());
            }

            return null;
        }

        private HtmlNode CreateDocumentAndRowNode(string html, string callerName)
        {
            HtmlDocument doc = CreateNodeDocument(html);

            return doc.DocumentNode.SelectSingleNode(_stringParser.GetXpath(callerName));
        }
    }
}
