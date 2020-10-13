using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;

namespace WebAPI.Services
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IProxyProvider _proxy;

        public HttpClientProvider(IConfiguration configuration, IProxyProvider proxy)
        {
            _configuration = configuration;
            _proxy = proxy;
        }

        public string GetHtmlDocument(string url)
        {
            string html = string.Empty;

            using (HttpResponseMessage response = GetNewHttpClient().GetAsync(url).Result)
            {
                using (HttpContent content = response.Content)
                {
                    html = content.ReadAsStringAsync().Result;
                }

                return html;
            }
        }

        private HttpClient GetNewHttpClient()
        {
            var proxy = new WebProxy
            {
                Address = new Uri($"http://{_proxy.GetIpAddress()}:{"80"}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName: _configuration["Proxy:User"], password: _configuration["Proxy:Pass"])
            };
            HttpClientHandler handler = new HttpClientHandler() { Proxy = proxy };
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/81.0");

            return client;
        }
    }
}
