namespace WebAPI.Services
{
    public interface IHttpClientProvider
    {
        string GetHtmlDocumentWithProxy(string url);
        string GetHtmlDocumentWithoutProxy(string url);
    }
}