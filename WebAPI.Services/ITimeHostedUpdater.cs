using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public interface ITimeHostedUpdater
    {
        Task StartAsync(CancellationToken token);
        Task StopAsync(CancellationToken source);
    }
}