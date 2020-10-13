using System;
using System.Net;
using System.Net.Http;

namespace WebAPI.Services
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private static HttpClient _client;

        public HttpClientProvider()
        {
            var proxy = new WebProxy
            {
                Address = new Uri($"http://{"185.30.232.123"}:{"80"}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName: "xzjoaonj-dest", password: "cci4lgnbjke8")
            };
            HttpClientHandler handler = new HttpClientHandler() { Proxy = proxy };
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/81.0");
        }

        public string GetHtmlDocument(string url)
        {
            string html = string.Empty;

            using (HttpResponseMessage response = _client.GetAsync(url).Result)
            {
                using (HttpContent content = response.Content)
                {
                    html = content.ReadAsStringAsync().Result;
                }

                return html;
            }
        }

        public string GetHtmlDocument2(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/81.0";
                string html = client.DownloadString(url);

                return html;
            }
        }
    }
}
