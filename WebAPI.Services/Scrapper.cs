using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class Scrapper : IScrapper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpService _http;

        public Scrapper(IConfiguration configuration, IHttpService http)
        {
            _configuration = configuration;
            _http = http;
        }

        public VesselUpdateModel ScrapSingleVessel(int mmsi, int imo, List<SeaModel> seaAreas)
        {
            VesselUpdateModel vessel = new VesselUpdateModel();

            //BASIC
            string scrapDestination = ""; //3 basic
            string scrapDraught = ""; //8 basic
            string scrapCourse_Speed = ""; //9 basic
            string scrapLat_Lon = ""; //10 basic
            string scrapSpeed = ""; //basic
            string scrapCourse = ""; //basic
            string scrapLatitude = ""; //basic
            string scrapLongitude = ""; //basic
            string scrapAisUpdateTime = ""; //basic
            string scrapEtaTime = ""; //4 basic
            string scrapNaviStatus = ""; //basic
            string scrapMmsi = "";



            string htmlDocument = _http.GetHtmlDocument(_configuration["Services:Url1"] + imo + ".html");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlDocument);


            HtmlNode naviStatus = doc.DocumentNode.SelectSingleNode("/html/body");

            if (naviStatus != null && naviStatus.OuterHtml.ToString().Contains("Navigational status:"))
            {
                scrapNaviStatus = naviStatus.OuterHtml.ToString().Split(new string[] { "Navigational status:</div><div class=" }, StringSplitOptions.None)[1].Split('>')[1].Split('<')[0].Trim();
                scrapNaviStatus = System.Net.WebUtility.HtmlDecode(scrapNaviStatus);
            }

            if (mmsi == 0 && string.IsNullOrEmpty(scrapMmsi))
            {
                HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("//div[@class='row']");

                if (rows != null)
                {
                    foreach (HtmlNode row in rows)
                    {
                        if (row.ChildNodes.Count > 2)
                        {
                        }
                        if (row.InnerHtml.ToString().Contains("MMSI:"))
                        {
                            scrapMmsi = row.ChildNodes[1].InnerText;
                            if (int.TryParse(scrapMmsi, out int i))
                            {
                                mmsi = int.Parse(scrapMmsi);
                            }
                        }
                        if (row.InnerHtml.ToString().Contains("Course:"))
                        {
                            if (row.InnerHtml.ToString().Contains("<span>"))
                                scrapCourse = row.ChildNodes[1].InnerText.Split(new string[] { "&deg" }, StringSplitOptions.None)[0];
                        }
                        if (row.InnerHtml.ToString().Contains("Last seen:"))
                        {
                            if (row.InnerHtml.ToString().Contains("<span>") && row.InnerHtml.ToString().Contains("<"))
                                scrapAisUpdateTime = row.InnerHtml.ToString()
                                .Split(new string[] { "<span>" }, StringSplitOptions.None)[1]
                                .Split('<')[0];
                        }
                    }
                }
            }
            if (mmsi != 0)
            {
                //VF vessel page

                htmlDocument = _http.GetHtmlDocument(_configuration["Services:Url2"] + mmsi);

                doc.LoadHtml(htmlDocument);

                for (int i = 1; i < 11; i++)
                {
                    HtmlNode row = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[" + i + "]/td[2]");
                    if (row != null && row.OuterHtml.ToString().Contains("<td class=\"v3\">"))
                    {
                        string rowText = row.OuterHtml.ToString().Split(new string[] { "<td class=\"v3\">" }, StringSplitOptions.None)[1].Split('<')[0];

                        switch (i)
                        {
                            case 3:
                                scrapDestination = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 4:
                                scrapEtaTime = ParseEta(System.Net.WebUtility.HtmlDecode(rowText).Trim());
                                break;
                            case 8:
                                scrapDraught = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 9:
                                scrapCourse_Speed = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            case 10:
                                scrapLat_Lon = System.Net.WebUtility.HtmlDecode(rowText).Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(scrapDraught) && scrapDraught.Contains(" m"))
                    scrapDraught = scrapDraught.Replace(" m", "");

                //
                HtmlNode feedTime = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/main/div/section[2]/div/div[1]/table/tbody/tr[12]/td[2]");
                if (feedTime != null && feedTime.OuterHtml.ToString().Contains(" UTC") && feedTime.OuterHtml.ToString().Contains("<td class=\"v3 tooltip expand\" data-title=\""))
                {
                    scrapAisUpdateTime = feedTime.OuterHtml.ToString().Split(new string[] { "<td class=\"v3 tooltip expand\" data-title=\"" }, StringSplitOptions.None)[1]
                        .Split(new string[] { " UTC" }, StringSplitOptions.None)[0].Trim();
                    scrapAisUpdateTime = System.Net.WebUtility.HtmlDecode(scrapAisUpdateTime);
                }
                if (scrapCourse_Speed != "-" && !string.IsNullOrEmpty(scrapCourse_Speed))
                {
                    scrapCourse = scrapCourse_Speed.Split('°')[0].Trim();
                    scrapSpeed = scrapCourse_Speed.Split('/')[1].Split('k')[0].Trim();
                }
                if (scrapLat_Lon != "-" && !string.IsNullOrEmpty(scrapLat_Lon))
                {
                    scrapLatitude = scrapLat_Lon.Split('/')[0];
                    scrapLongitude = scrapLat_Lon.Split('/')[1];
                }

                if (scrapLatitude.Contains("N")) scrapLatitude = scrapLatitude.Split(' ')[0].Trim(); else if (scrapLatitude.Contains("S")) scrapLatitude = "-" + scrapLatitude.Split(' ')[0].Trim();
                if (scrapLongitude.Contains("E")) scrapLongitude = scrapLongitude.Split(' ')[0].Trim(); else if (scrapLongitude.Contains("W")) scrapLongitude = "-" + scrapLongitude.Split(' ')[0].Trim();
            }


            //Parsing with model below
            double d;
            DateTime date;

            vessel.IMO = imo;
            if (!string.IsNullOrEmpty(scrapMmsi)) vessel.MMSI = int.Parse(scrapMmsi);
            if (!string.IsNullOrEmpty(scrapDestination)) vessel.Destination = scrapDestination;
            if (!string.IsNullOrEmpty(scrapNaviStatus)) vessel.AISStatus = scrapNaviStatus; else vessel.AISStatus = null;
            if (!string.IsNullOrEmpty(scrapLatitude))
                if (double.TryParse(scrapLatitude, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                    vessel.Lat = double.Parse(scrapLatitude, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(scrapLongitude))
                if (double.TryParse(scrapLongitude, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                    vessel.Lon = double.Parse(scrapLongitude, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(scrapSpeed))
                if (double.TryParse(scrapSpeed, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                    vessel.Speed = double.Parse(scrapSpeed, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(scrapCourse))
                if (double.TryParse(scrapCourse, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                    vessel.Course = double.Parse(scrapCourse, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(scrapDraught))
                if (double.TryParse(scrapDraught, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out d))
                    vessel.Draught = double.Parse(scrapDraught, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(scrapAisUpdateTime))
            {
                if (DateTime.TryParse(scrapAisUpdateTime, out date))
                {
                    vessel.AISLatestActivity = DateTime.Parse(scrapAisUpdateTime);

                    if (vessel.AISLatestActivity.Value < DateTime.UtcNow.AddDays(-2))
                    {
                        vessel.AISStatus = "(out-of-date)";
                    }
                }
            }
            else
            {
                vessel.AISStatus = "Undefined";
            }
            if (!string.IsNullOrEmpty(scrapEtaTime))
                if (DateTime.TryParse(scrapEtaTime, out date))
                    vessel.ETA = DateTime.Parse(scrapEtaTime);
            vessel.AISStatus = VerifyAisStatus(vessel.AISStatus, vessel.Speed);

            if (vessel.Lat != 0 && vessel.Lat.HasValue && vessel.Lon != 0 && vessel.Lon.HasValue)
            {
                vessel.GeographicalArea = GetGeographicalArea(vessel.Lat, vessel.Lon, seaAreas);
            }

            if (vessel.Speed != null)
            {

            }

            return vessel;
        }

        private string GetGeographicalArea(double? lat, double? lon, List<SeaModel> areas)
        {
            string result = "";

            foreach (SeaModel area in areas)
            {
                MapPointModel point = new MapPointModel() { Lat = double.Parse(lat.ToString()), Lon = double.Parse(lon.ToString()) };

                if (VerifyPolygon(point, area))
                    return area.Name;
            }

            return result;
        }

        private bool VerifyPolygon(MapPointModel point, SeaModel area)
        {
            if (point.Lat > area.MinLatitude && point.Lat < area.MaxLatitude && point.Lon > area.MinLongitude && point.Lon < area.MaxLongitude)
                return true;

            return false;
        }

        private string FilterTypes(string type)
        {
            if (type.Contains(" ship")) type = type.Replace(" ship", "");
            if (type.Contains(" vessel")) type = type.Replace(" vessel", "");
            if (type.Contains(" tanker")) type = type.Replace(" tanker", "");
            if (type.Contains("passengers")) type = type.Replace("passenger", "");
            if (type.Contains(" (cruise)")) type = type.Replace(" (cruise)", " cruise");

            return type;
        }

        private string VerifyAisStatus(string aISStatus, double? speed)
        {
            string finalStatus = aISStatus;

            if (speed != null)
            {
                if ((aISStatus == "Moving" || aISStatus == "Sailing" || aISStatus == "Fishing") && speed.Value < 0.5)
                {
                    aISStatus = "Not Moving";
                }
                else if ((aISStatus == "Moored" || aISStatus == "Anchored") && speed.Value > 0.5)
                {
                    aISStatus = "Under way";
                }
                else if (aISStatus == "Moving" && speed.Value >= 0.5)
                {
                    aISStatus = "Under way";
                }
                else return aISStatus;
            }

            return aISStatus;
        }

        private static string ParseEta(string etaVF)
        {
            if (!string.IsNullOrEmpty(etaVF) && etaVF.Contains(",") && etaVF.Contains(" "))
            {
                string month = etaVF.Split(' ')[0].Trim();
                switch (month)
                {
                    case "Jan":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 01");
                        break;
                    case "Feb":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 02");
                        break;
                    case "Mar":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 03");
                        break;
                    case "Apr":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 04");
                        break;
                    case "May":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 05");
                        break;
                    case "Jun":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 06");
                        break;
                    case "Jul":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 07");
                        break;
                    case "Aug":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 08");
                        break;
                    case "Sep":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 09");
                        break;
                    case "Oct":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 10");
                        break;
                    case "Nov":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 11");
                        break;
                    case "Dec":
                        etaVF = etaVF.Split(' ')[1].Replace(",", " 12");
                        break;
                    default:
                        break;
                }
            }

            return etaVF;
        }
    }
}
