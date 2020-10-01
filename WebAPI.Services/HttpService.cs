using System.Net.Http;

namespace WebAPI.Services
{
    public class HttpService : IHttpService
    {
        private static HttpClient _httpClient;

        private HttpClient GetHttpClient()
        {
            if (_httpClient != null)
            {
                return _httpClient;
            }

            return CreateNewHttpClient();
        }

        private static HttpClient CreateNewHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3703.0 Safari/537.36");

            return client;
        }

        public string GetHtmlDocument(string url)
        {
            string html = string.Empty;

            using (HttpResponseMessage response = GetHttpClient().GetAsync(url).Result)
            {
                using (HttpContent content = response.Content)
                {
                    html = content.ReadAsStringAsync().Result;
                }
            }

            return html;
        }
    }
}
