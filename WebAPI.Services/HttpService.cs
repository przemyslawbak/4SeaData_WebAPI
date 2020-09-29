using System.Net.Http;

namespace WebAPI.Services
{
    public class HttpService : IHttpService
    {
        public HttpClient GetNewHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3703.0 Safari/537.36");

            return client;
        }
    }
}
