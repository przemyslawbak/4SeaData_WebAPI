using HtmlAgilityPack;
using System;
using System.Net;
using System.Reflection;

namespace WebAPI.Services
{
    public class NodeParser : INodeParser
    {
        private readonly IStringParser _stringParser;

        public NodeParser(IStringParser stringParser)
        {
            _stringParser = stringParser;
        }

        public double? ExtractCourseFromHtml(string html_document_2)
        {
            string rowText = CreatePrepareAndVerifyDocument(html_document_2);

            if (!string.IsNullOrEmpty(rowText))
            {
                return _stringParser.ParsedNullableDouble(_stringParser.GetTrimmedCourse(rowText));
            }

            return null;
        }

        public string ExtractDestinationFromHtml(string html_document_2)
        {
            string rowText = CreatePrepareAndVerifyDocument(html_document_2);

            if (!string.IsNullOrEmpty(rowText))
            {
                string text = _stringParser.GetTrimmedText(rowText);

                if (!string.IsNullOrEmpty(text) && text != "-")
                {
                    return text;
                }
            }

            return null;
        }

        public double? ExtractDraughtFromHtml(string html_document_2)
        {
            string rowText = CreatePrepareAndVerifyDocument(html_document_2);

            if (!string.IsNullOrEmpty(rowText))
            {
                string text = _stringParser.GetTrimmedText(rowText);

                if (!string.IsNullOrEmpty(text) && text.Contains(" m"))
                {
                    text = text.Replace(" m", "");

                    return _stringParser.ParsedNullableDouble(text);

                }
            }

            return null;
        }

        public DateTime? ExtractEtaTimeFromHtml(string html_document_2)
        {
            string rowText = CreatePrepareAndVerifyDocument(html_document_2);

            if (!string.IsNullOrEmpty(rowText))
            {
                return _stringParser.ParsedNullableDateTime(_stringParser.GetTrimmedCourse(rowText));
            }

            return null;
        }

        public double? ExtractLatFromHtml(string html_document_2)
        {
            string rowText = CreatePrepareAndVerifyDocument(html_document_2);

            if (!string.IsNullOrEmpty(rowText))
            {
                string text = _stringParser.GetTrimmedText(rowText);

                if (text != "-" && !string.IsNullOrEmpty(text))
                {
                    string lat = text.Split('/')[0];
                    if (lat.Contains("N")) lat = lat.Split(' ')[0].Trim(); else if (lat.Contains("S")) lat = "-" + lat.Split(' ')[0].Trim();

                    return _stringParser.ParsedNullableDouble(lat);
                }
            }

            return null;
        }

        public double? ExtractLonFromHtml(string html_document_2)
        {
            string rowText = CreatePrepareAndVerifyDocument(html_document_2);

            if (!string.IsNullOrEmpty(rowText))
            {
                string text = _stringParser.GetTrimmedText(rowText);

                if (text != "-" && !string.IsNullOrEmpty(text))
                {
                    string lon = text.Split('/')[1];
                    if (lon.Contains("E")) lon = lon.Split(' ')[0].Trim(); else if (lon.Contains("W")) lon = "-" + lon.Split(' ')[0].Trim();

                    return _stringParser.ParsedNullableDouble(lon);
                }
            }

            return null;
        }

        public double? ExtractSpeedFromHtml(string html_document_2)
        {
            string rowText = CreatePrepareAndVerifyDocument(html_document_2);

            if (!string.IsNullOrEmpty(rowText))
            {
                return _stringParser.ParsedNullableDouble(_stringParser.GetTrimmedCourse(rowText));
            }

            return null;
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

        private string CreatePrepareAndVerifyDocument(string html_document_2)
        {
            HtmlDocument doc = CreateNodeDocument(html_document_2);

            HtmlNode row = doc.DocumentNode.SelectSingleNode(_stringParser.GetXpath(MethodBase.GetCurrentMethod().Name));
            if (_stringParser.IsTableRowCorrect(row.OuterHtml.ToString()))
            {
                return _stringParser.SplitRow(row.OuterHtml.ToString());
            }

            return null;
        }
    }
}
