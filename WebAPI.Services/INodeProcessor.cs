using System;

namespace WebAPI.Services
{
    public interface INodeProcessor
    {
        string ExtractDestinationFromHtml(string html_document_2);
        double? ExtractCourseFromHtml(string html_document_2);
        double? ExtractSpeedFromHtml(string html_document_2);
        double? ExtractDraughtFromHtml(string html_document_2);
        int ExtractMmsiFromHtml(string html_document_1);
        DateTime? ExtractAisUpdateTimeFromHtml(string html_document_1, string html_document_2);
        DateTime? ExtractEtaTimeFromHtml(string html_document_2);
        double? ExtractLatFromHtml(string html_document_2);
        double? ExtractLonFromHtml(string html_document_2);
        string ExtractNaviStatusFromHtml(string html_document_1, DateTime? time);
    }
}