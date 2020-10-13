using System;
using System.Net.Http;

namespace WebAPI.Services
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private static HttpClient _httpClient = new HttpClient();

        public HttpClientProvider()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/81.0");
            _httpClient.Timeout = new TimeSpan(0, 0, 5);
        }

        public string GetHtmlDocument(string url)
        {
            string html = string.Empty;

            HttpResponseMessage response = _httpClient.GetAsync(url).Result;

            using (HttpContent content = response.Content)
            {
                html = content.ReadAsStringAsync().Result;
            }

            return html;
        }
    }
}
