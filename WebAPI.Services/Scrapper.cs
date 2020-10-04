using Microsoft.Extensions.Configuration;
using System;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class Scrapper : IScrapper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientProvider _http;
        private readonly INodeParser _nodeParser;
        private readonly IGeoAreaFinder _areaFinder;

        public Scrapper(IConfiguration configuration, IHttpClientProvider http, INodeParser nodeParser, IGeoAreaFinder areaFinder)
        {
            _configuration = configuration;
            _http = http;
            _nodeParser = nodeParser;
            _areaFinder = areaFinder;
        }

        public VesselUpdateModel ScrapSingleVessel(int mmsi, int imo)
        {
            string html_document_1 = GetHtml1(imo);
            string html_document_2 = GetHtml2(mmsi, imo, html_document_1);

            return GetVesselUpdates(html_document_1, html_document_2, imo, mmsi);
        }

        private string GetHtml2(int mmsi, int imo, string html_document_1)
        {
            if (mmsi == 0)
            {
                mmsi = _nodeParser.ExtractMmsiFromHtml(html_document_1, imo);
            }

            return _http.GetHtmlDocument(_configuration["Services:Url2"] + mmsi);
        }

        private string GetHtml1(int imo)
        {
            return _http.GetHtmlDocument(_configuration["Services:Url1"] + imo + ".html");
        }

        private VesselUpdateModel GetVesselUpdates(string html_document_1, string html_document_2, int imo, int mmsi)
        {
            double? lat = _nodeParser.ExtractLatFromHtml(html_document_2);
            double? lon = _nodeParser.ExtractLonFromHtml(html_document_2);
            DateTime? time = _nodeParser.ExtractAisUpdateTimeFromHtml(html_document_1, html_document_2);

            VesselUpdateModel vessel = new VesselUpdateModel()
            {
                Destination = _nodeParser.ExtractDestinationFromHtml(html_document_2),
                AISStatus = _nodeParser.ExtractNaviStatusFromHtml(html_document_1, time),
                ETA = _nodeParser.ExtractEtaTimeFromHtml(html_document_2),
                Course = _nodeParser.ExtractCourseFromHtml(html_document_2),
                Speed = _nodeParser.ExtractSpeedFromHtml(html_document_2),
                Draught = _nodeParser.ExtractDraughtFromHtml(html_document_2),
                AISLatestActivity = time,
                Lat = lat,
                Lon = lon,
                IMO = imo,
                MMSI = mmsi
            };

            vessel.GeographicalArea = _areaFinder.GetGeographicalArea(lat, lon);

            return vessel;
        }
    }
}
