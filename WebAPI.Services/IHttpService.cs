using System.Net.Http;

namespace WebAPI.Services
{
    public interface IHttpService
    {
        HttpClient GetNewHttpClient();
    }
}