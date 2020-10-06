using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class StringParser : IStringParser
    {
        public string BuildUpdatedVesselInfo(VesselUpdateModel result)
        {
            if (result != null) //todo: unit test
            {
                return BuildResult(result);
            }

            else return null;
        }

        public string GetTrimmedCourse(string rowText)
        {
            return DecodeAndTrim(rowText).Split('°')[0].Trim();
        }

        public string GetTrimmedSpeed(string rowText)
        {
            return DecodeAndTrim(rowText).Split('/')[1].Split('k')[0].Trim();
        }

        public string GetTrimmedText(string rowText)
        {
            return DecodeAndTrim(rowText);
        }

        public string GetXpath(string methodName)
        {
            Dictionary<string, string> xpathDict = new Dictionary<string, string>()
            {
                { "ExtractCourseFromHtml", "/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[9]/td[2]" },
                { "ExtractDestinationFromHtml", "/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[3]/td[2]" },
                { "ExtractDraughtFromHtml", "/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[8]/td[2]" },
                { "ExtractEtaTimeFromHtml", "/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[4]/td[2]" },
                { "ExtractLatFromHtml", "/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[10]/td[2]" },
                { "ExtractLonFromHtml", "/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[10]/td[2]" },
                { "ExtractSpeedFromHtml", "/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[9]/td[2]" },
                { "ExtractMmsiFromHtml", "//div[@class='row']" },
                { "ExtractNaviStatusFromHtml", "/html/body" },
                { "ExtractAisUpdateTimeFromHtml1", "//div[@class='row']" },
                { "ExtractAisUpdateTimeFromHtml2", "/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[12]/td[2]" },
            };

            return xpathDict["methodName"];
        }

        public bool IsTableRowCorrect(string row)
        {
            if (string.IsNullOrEmpty(row))
            {
                if (row.Contains("<td class=\"v3\">"))
                {
                    return true;
                }
            }

            return false;
        }

        public DateTime? ParsedNullableDateTime(string eta)
        {
            if (DateTime.TryParse(eta, out DateTime date))
            {
                return date;
            }

            return null;
        }

        public double? ParsedNullableDouble(string speed)
        {
            if (double.TryParse(speed, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double d))
            {
                return d;
            }

            return null;
        }

        public int ParsedInt(string mmsi)
        {
            if (int.TryParse(mmsi, out int i))
            {
                return i;
            }

            return 0;
        }

        public string SplitRow(string outerHtml)
        {
            return outerHtml.Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
        }

        private string BuildResult(VesselUpdateModel result)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(result.IMO);
            sb.Append(" | ");
            if (result.AISLatestActivity.HasValue)
            {
                sb.Append(result.AISLatestActivity.Value.ToShortDateString());
            }
            else
            {
                sb.Append("(no activity time)");
            }
            sb.Append(" | ");
            if (!string.IsNullOrEmpty(result.AISStatus))
            {
                sb.Append(result.AISStatus);
            }
            else
            {
                sb.Append("(no status)");
            }
            sb.Append(" | ");
            if (!string.IsNullOrEmpty(result.Destination))
            {
                sb.Append(result.Destination);
            }
            else
            {
                sb.Append("(no destination)");
            }

            return sb.ToString();
        }

        private string DecodeAndTrim(string rowText)
        {
            return WebUtility.HtmlDecode(rowText).Trim();
        }
    }
}
