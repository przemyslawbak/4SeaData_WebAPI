using System.Collections.Generic;
using System.Net.Http;

namespace WebAPI.Services
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IProxyProvider _proxy;
        private static HttpClient _sharedClient = new HttpClient();
        private static int _proxyNumber;

        public HttpClientProvider(IProxyProvider proxy, IHttpClientFactory httpClientFactory)
        {
            _proxy = proxy;
            _httpClientFactory = httpClientFactory;
        }

        public string GetHtmlDocumentWithoutProxy(string url)
        {
            string html = string.Empty;

            using (HttpResponseMessage response = _sharedClient.GetAsync(url).Result)
            {
                using (HttpContent content = response.Content)
                {
                    html = content.ReadAsStringAsync().Result;
                }

                return html;
            }
        }

        public string GetHtmlDocumentWithProxy(string url)
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
            List<string> proxies = _proxy.GetProxies();
            HttpClient client = _httpClientFactory.CreateClient(proxies[_proxyNumber]);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Android 11; Mobile; rv:68.0) Gecko/68.0 Firefox/85.0");

            _proxyNumber = UpdateProxyNumber(_proxyNumber);

            return client;
        }

        private int UpdateProxyNumber(int proxyNumber)
        {
            if (_proxyNumber == 99) return 0;

            return proxyNumber++;
        }
    }
}
