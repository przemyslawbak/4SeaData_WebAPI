using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Net;

namespace WebAPI.Services
{
    public class NodeParser : INodeParser
    {
        public DateTime? ExtractAisUpdateTimeFromHtml(string html_document_1, string html_document_2)
        {
            DateTime? time1 = null;
            DateTime? time2 = null;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_1);

            HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("//div[@class='row']");
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
                                time1 = DateTime.Parse(time);
                            }
                        }
                    }
                }
            }

            doc.LoadHtml(html_document_2);
            HtmlNode feedTime = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[12]/td[2]");
            if (feedTime != null && feedTime.OuterHtml.ToString().Contains(" UTC") && feedTime.OuterHtml.ToString().Contains("<td class=\"v3 tooltip expand\" data-title=\""))
            {
                string time = feedTime.OuterHtml.ToString().Split(new string[] { "<td class=\"v3 tooltip expand\" data-title=\"" }, StringSplitOptions.None)[1]
                    .Split(new string[] { " UTC" }, StringSplitOptions.None)[0].Trim();

                if (DateTime.TryParse(time, out DateTime date))
                {
                    time2 = DateTime.Parse(time);
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

        public double? ExtractCourseFromHtml(string html_document_2)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_2);

            HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[9]/td[2]");
            if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
            {
                string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
                string course = WebUtility.HtmlDecode(rowText).Trim().Split('°')[0].Trim();

                if (double.TryParse(course, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double d))
                {
                    return double.Parse(course, CultureInfo.InvariantCulture);
                }
            }

            return null;
        }

        public string ExtractDestinationFromHtml(string html_document_2)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_2);

            HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[3]/td[2]");
            if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
            {
                string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
                string destination = WebUtility.HtmlDecode(rowText).Trim();

                if (string.IsNullOrEmpty(destination))
                {
                    return destination;
                }
            }

            return null;
        }

        public double? ExtractDraughtFromHtml(string html_document_2)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_2);

            HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[8]/td[2]");
            if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
            {
                string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
                string draught = WebUtility.HtmlDecode(rowText).Trim();

                if (string.IsNullOrEmpty(draught) && draught.Contains(" m"))
                {
                    draught = draught.Replace(" m", "");

                    if (double.TryParse(draught, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double d))
                    {
                        return double.Parse(draught, CultureInfo.InvariantCulture);
                    }
                     
                }
            }

            return null;
        }

        public DateTime? ExtractEtaTimeFromHtml(string html_document_2)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_2);

            HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[4]/td[2]");
            if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
            {
                string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
                string eta = WebUtility.HtmlDecode(rowText).Trim();

                if (DateTime.TryParse(eta, out DateTime date))
                {
                    return DateTime.Parse(eta);
                }
            }

            return null;
        }

        public double? ExtractLatFromHtml(string html_document_2)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_2);

            HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[8]/td[2]");
            if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
            {
                string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
                string lat_lon = WebUtility.HtmlDecode(rowText).Trim();

                if (lat_lon != "-" && !string.IsNullOrEmpty(lat_lon))
                {
                    string lat = lat_lon.Split('/')[0];
                    if (lat.Contains("N")) lat = lat.Split(' ')[0].Trim(); else if (lat.Contains("S")) lat = "-" + lat.Split(' ')[0].Trim();

                    if (double.TryParse(lat, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double d))
                    {
                        return double.Parse(lat, CultureInfo.InvariantCulture);
                    }
                }
            }

            return null;
        }

        public double? ExtractLonFromHtml(string html_document_2)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_2);

            HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[8]/td[2]");
            if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
            {
                string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
                string lat_lon = WebUtility.HtmlDecode(rowText).Trim();

                if (lat_lon != "-" && !string.IsNullOrEmpty(lat_lon))
                {
                    string lon = lat_lon.Split('/')[1];
                    if (lon.Contains("E")) lon = lon.Split(' ')[0].Trim(); else if (lon.Contains("W")) lon = "-" + lon.Split(' ')[0].Trim();

                    if (double.TryParse(lon, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double d))
                    {
                        return double.Parse(lon, CultureInfo.InvariantCulture);
                    }
                }
            }

            return null;
        }

        public int ExtractMmsiFromHtml(string html_document_1, int imo)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_1);

            HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("//div[@class='row']");
            if (rows != null)
            {
                foreach (HtmlNode row in rows)
                {
                    if (row.InnerHtml.ToString().Contains("MMSI:"))
                    {
                        string mmsi = row.ChildNodes[1].InnerText;
                        if (int.TryParse(mmsi, out int i))
                        {
                            return int.Parse(mmsi);
                        }
                    }
                }
            }

            return 0;
        }

        public string ExtractNaviStatusFromHtml(string html_document_1, DateTime? aISLatestActivity)
        {
            string aISStatus = null;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_1);

            HtmlNode naviStatus = doc.DocumentNode.SelectSingleNode("/html/body");
            if (naviStatus != null && naviStatus.OuterHtml.ToString().Contains("Navigational status:"))
            {
                aISStatus = naviStatus.OuterHtml.ToString().Split(new string[] { "Navigational status:</div><div class=" }, StringSplitOptions.None)[1].Split('>')[1].Split('<')[0].Trim();
                aISStatus = WebUtility.HtmlDecode(aISStatus);
            }

            if (aISLatestActivity.HasValue)
            {
                if (aISLatestActivity.Value < DateTime.UtcNow.AddDays(-2))
                {
                    return "(out-of-date)";
                }
            }

            return aISStatus;
        }

        public double? ExtractSpeedFromHtml(string html_document_2)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html_document_2);

            HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[9]/td[2]");
            if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
            {
                string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
                string speed = WebUtility.HtmlDecode(rowText).Trim().Split('/')[1].Split('k')[0].Trim();

                if (double.TryParse(speed, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double d))
                {
                    return double.Parse(speed, CultureInfo.InvariantCulture);
                }
            }

            return null;
        }
    }
}
