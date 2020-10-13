using System.Collections.Generic;

namespace WebAPI.Services
{
    public interface IProxyProvider
    {
        List<string> GetProxies();
    }
}