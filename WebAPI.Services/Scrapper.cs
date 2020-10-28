using Microsoft.Extensions.Configuration;
using System;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class Scrapper : IScrapper
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientProvider _http;
        private readonly INodeProcessor _nodeProcessor;
        private readonly IGeoAreaFinder _areaFinder;

        public Scrapper(IConfiguration configuration, IHttpClientProvider http, INodeProcessor nodeParser, IGeoAreaFinder areaFinder)
        {
            _configuration = configuration;
            _http = http;
            _nodeProcessor = nodeParser;
            _areaFinder = areaFinder;
        }

        public VesselUpdateModel ScrapSingleVessel(int mmsi, int imo)
        {
            string html_document_1 = GetHtml1(imo);
            string html_document_2 = GetHtml2(mmsi, html_document_1);

            return GetVesselUpdates(html_document_1, html_document_2, imo, mmsi);
        }

        private string GetHtml1(int imo)
        {
            return _http.GetHtmlDocumentWithoutProxy(_configuration["Services:Url1"] + imo + ".html");
        }

        private string GetHtml2(int mmsi, string html_document_1) //todo: unit test
        {
            int newMmsi = _nodeProcessor.ExtractMmsiFromHtml(html_document_1);

            if (newMmsi != 0) mmsi = newMmsi;

            if (mmsi == 0) return "";

            return _http.GetHtmlDocumentWithProxy(_configuration["Services:Url2"] + mmsi);
        }

        private VesselUpdateModel GetVesselUpdates(string html_document_1, string html_document_2, int imo, int mmsi)
        {
            double? lat = _nodeProcessor.ExtractLatFromHtml(html_document_2);
            double? lon = _nodeProcessor.ExtractLonFromHtml(html_document_2);
            DateTime? time = _nodeProcessor.ExtractAisUpdateTimeFromHtml(html_document_1, html_document_2);

            VesselUpdateModel vessel = new VesselUpdateModel()
            {
                Destination = _nodeProcessor.ExtractDestinationFromHtml(html_document_2),
                AISStatus = _nodeProcessor.ExtractNaviStatusFromHtml(html_document_1, time),
                ETA = _nodeProcessor.ExtractEtaTimeFromHtml(html_document_2),
                Course = _nodeProcessor.ExtractCourseFromHtml(html_document_2),
                Speed = _nodeProcessor.ExtractSpeedFromHtml(html_document_2),
                Draught = _nodeProcessor.ExtractDraughtFromHtml(html_document_2),
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
