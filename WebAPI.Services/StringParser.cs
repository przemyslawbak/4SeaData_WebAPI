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

        public string BuildUpdatedVesselInfo(VesselUpdateModel result)
        {
            if (result != null) //todo: unit test
            {
                return BuildResult(result);
            }
            
            return null;
        }

        public string GetUndashedDestination(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            return DecodeAndTrim(text);
        }

        public string GetTrimmedCourse(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            return DecodeAndTrim(text).Split('°')[0].Trim();
        }

        public string GetTrimmedSpeed(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            return DecodeAndTrim(text).Split('/')[1].Split('k')[0].Trim();
        }


        public string GetTrimmedLongitude(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            string trimmed = GetTrimmedText(text).Split('/')[0];

            if (trimmed.Contains("E"))
            {
                return trimmed.Split(' ')[0].Trim();
            }
            else if (trimmed.Contains("W"))
            {
                return "-" + trimmed.Split(' ')[0].Trim();
            }

            return null;
        }

        public string GetTrimmedLatitude(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            string trimmed = GetTrimmedText(text).Split('/')[0];

            if (trimmed.Contains("N"))
            {
                return trimmed.Split(' ')[0].Trim();
            }
            else if (trimmed.Contains("S"))
            {
                return "-" + trimmed.Split(' ')[0].Trim();
            }

            return null;
        }

        public string GetTrimmedText(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            if (text != "-")
            {
                return DecodeAndTrim(text);
            }

            return null;
        }

        public string GetTrimmedDraught(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            if (text.Contains(" m"))
            {
                return GetTrimmedText(text).Replace(" m", "");
            }

            return null;
        }

        public bool IsTableRowCorrect(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;

            if (text.Contains("<td class=\"v3\">"))
            {
                return true;
            }

            return false;
        }

        public DateTime? ParsedTrimmedNullableDateTime(string text)
        {
            if (DateTime.TryParse(text.Trim(), out DateTime date))
            {
                return date;
            }

            return null;
        }

        public double? ParsedTrimmedNullableDouble(string text)
        {
            if (double.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double d))
            {
                return d;
            }

            return null;
        }

        public int ParsedInt(string text)
        {
            if (int.TryParse(text, out int i))
            {
                return i;
            }

            return 0;
        }

        public string SplitRow(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            return text.Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];
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

        private string DecodeAndTrim(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            return WebUtility.HtmlDecode(text).Trim();
        }
    }
}
