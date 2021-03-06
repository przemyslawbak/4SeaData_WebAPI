﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class StringParser : IStringParser
    {
        public string GetXpath(string methodName)
        {
            Dictionary<string, string> xpathDict = new Dictionary<string, string>()
            {
                { "ExtractCourseFromHtml", "/html/body/div[1]/div/main/div/section[1]/div/div[2]/div/div[2]/table/tbody/tr[1]/td[2]" },
                { "ExtractDestinationFromHtml", "/html/body/div[1]/div/main/div/section[1]/div/div[2]/div/div[1]/div[2]" },
                { "ExtractDraughtFromHtml", "/html/body/div[1]/div/main/div/section[1]/div/div[2]/div/div[2]/table/tbody/tr[2]/td[2]" },
                { "ExtractEtaTimeFromHtml", "/html/body/div[1]/div/main/div/section[1]/div/div[2]/div/div[1]/div[2]/div/span[1]" },
                { "ExtractLatFromHtml", "/html/body/div[1]/div/main/div/div[3]/p[1]" },
                { "ExtractLonFromHtml", "/html/body/div[1]/div/main/div/div[3]/p[1]" },
                { "ExtractSpeedFromHtml", "/html/body/div[1]/div/main/div/section[1]/div/div[2]/div/div[2]/table/tbody/tr[1]/td[2]" },
                { "ExtractMmsiFromHtml", "//div[@class='row']" },
                { "ExtractNaviStatusFromHtml", "/html/body" },
                { "ExtractAisUpdateTimeFromHtml1", "//div[@class='row']" },
                { "ExtractAisUpdateTimeFromHtml2", "//*[@id='lastrep']" },
            };

            string result = xpathDict[methodName];

            return result;
        }
        public string MakeFirstCharactersToUpper(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = Regex.Replace(text.Trim().ToLower(), @"(^\w)|(\s\w)", m => m.Value.ToUpper());
                if (text.Contains("'"))
                    text = text.Replace("'", "`");
            }

            return text;
        }

        public string BuildUpdatedVesselInfo(VesselUpdateModel result)
        {
            if (result != null)
            {
                return BuildResult(result);
            }
            
            return null;
        }

        public string GetTrimmedMmsi(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains("MMSI:")) return null;

            return DecodeAndTrim(text).Split(new string[] { "MMSI:" }, StringSplitOptions.None)[1]
                .Split('>')[2]
                .Split('<')[0]
                .Trim();
        }

        public string GetTrimmedTime2(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains("<span>")) return null;

            return DecodeAndTrim(text).Split(new string[] { "<span>" }, StringSplitOptions.None)[1]
                            .Split('<')[0]
                            .Trim();
        }

        public string GetTrimmedTime1(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains(" UTC")) return null;
            if (!text.Contains("<td class=\"v3 tooltip expand\" data-title=\"")) return null;

            return DecodeAndTrim(text).Split(new string[] { "<td class=\"v3 tooltip expand\" data-title=\"" }, StringSplitOptions.None)[1]
                    .Split(new string[] { " UTC" }, StringSplitOptions.None)[0]
                    .Trim();
        }

        public string GetAisStatusTrimmed(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains("Navigational status:</div><div class=")) return null;

            return DecodeAndTrim(text).Split(new string[] { "Navigational status:</div><div class=" }, StringSplitOptions.None)[1]
                .Split('>')[1]
                .Split('<')[0]
                .Trim();

        }

        public string GetUndashedTrimmedText(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (text != "-")
            {
                return DecodeAndTrim(text);
            }

            return null;
        }

        public string GetTrimmedCourse(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!DecodeAndTrim(text).Contains("°")) return null;

            return DecodeAndTrim(text)
                .Split('°')[0]
                .Trim(); ;
        }

        public string GetTrimmedSpeed(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains("k")) return null;
            if (!text.Contains("/")) return null;

            return DecodeAndTrim(text)
                .Split('/')[1]
                .Split('k')[0]
                .Trim();
        }

        public string GetTrimmedDestination(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (text.Contains("Destination not available")) return null;
            if (!text.Contains(",")) return null;

            return text.Split(',')[0].Trim();
        }

        public string GetTrimmedEta(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains("ATA: ")) return null;
            if (!text.Contains(" UTC")) return null;

            return text.Split(new string[] { "ATA: " }, StringSplitOptions.None)[1].Split(new string[] { " UTC" }, StringSplitOptions.None)[0];
        }

        public string GetTrimmedLongitude(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains("coordinates ")) return null;
            if (!text.Contains("/")) return null;
            if (!text.Contains(" ")) return null;
            if (!text.Contains("(")) return null;
            if (!text.Contains(")")) return null;
            text = text.Replace("coordinates ", "");
            string trimmed = DecodeAndTrim(text)
                .Split('(')[1]
                .Split(')')[0]
                .Split('/')[1].Trim();
            if (trimmed.Contains("E"))
            {
                return trimmed
                    .Split(' ')[0]
                    .Trim();
            }
            else if (trimmed.Contains("W"))
            {
                return "-" + trimmed
                    .Split(' ')[0]
                    .Trim();
            }

            return null;
        }

        public string GetTrimmedLatitude(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains("coordinates ")) return null;
            if (!text.Contains("/")) return null;
            if (!text.Contains(" ")) return null;
            if (!text.Contains("(")) return null;
            if (!text.Contains(")")) return null;
            text = text.Replace("coordinates ", "");
            string trimmed = DecodeAndTrim(text)
                .Split('(')[1]
                .Split(')')[0]
                .Split('/')[0].Trim();
            if (trimmed.Contains("N"))
            {
                return trimmed
                    .Split(' ')[0]
                    .Trim();
            }
            else if (trimmed.Contains("S"))
            {
                return "-" + trimmed
                    .Split(' ')[0]
                    .Trim();
            }

            return null;
        }

        public string GetTrimmedDraught(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (text.Contains(" m"))
            {
                return DecodeAndTrim(text)
                    .Replace(" m", "");
            }

            return null;
        }

        public string SplitTableRow(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (!text.Contains("<td class=\"v3\">")) return null;

            return text
                .Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1]
                .Split('<')[0]
                .Trim();
        }

        public bool IsTableRowCorrect(string text)
        {
            return true;

            if (string.IsNullOrEmpty(text)) return false;
            if (text.Contains("<td class=\"v3\">"))
            {
                return true;
            }

            return false;
        }

        public bool IsContainingMmsi(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (text.Contains("MMSI:"))
            {
                return true;
            }

            return false;
        }

        public bool IsRowTimeRow(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (!text.Contains("Last seen:")) return false;

            if (text.Contains("<span>"))
            {
                return true;
            }

            return false;
        }

        public double? ParsedTrimmedNullableDouble(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (double.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out double d))
            {
                return d;
            }

            return null;
        }

        public int ParsedInt(string text)
        {
            if (string.IsNullOrEmpty(text)) return 0;
            if (int.TryParse(text, out int i))
            {
                return i;
            }

            return 0;
        }

        public DateTime? ParsedTrimmedNullableDateTime(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (DateTime.TryParse(text.Trim(), out DateTime date))
            {
                return date;
            }

            return null;
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
