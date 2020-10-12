using System;
using System.Net.Http;

namespace WebAPI.Services
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private static HttpClient _httpClient;

        public string GetHtmlDocument(string url)
        {
            string html = string.Empty;

            using (HttpResponseMessage response = GetHttpClient().GetAsync(new Uri(url, UriKind.Absolute)).Result)
            {
                using (HttpContent content = response.Content)
                {
                    html = content.ReadAsStringAsync().Result;
                }
            }

            return html;
        }

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
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3703.0 Safari/537.36");

            return _httpClient;
        }
    }
}
