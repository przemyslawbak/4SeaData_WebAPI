namespace WebAPI.Services
{
    public interface IHttpClientProvider
    {
        string GetHtmlDocument(string html);
    }
}