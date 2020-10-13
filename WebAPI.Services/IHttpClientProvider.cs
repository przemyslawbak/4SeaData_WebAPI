namespace WebAPI.Services
{
    public interface IHttpClientProvider
    {
        string GetHtmlDocumentWithProxy(string html);
        string GetHtmlDocumentWithoutProxy(string v);
    }
}