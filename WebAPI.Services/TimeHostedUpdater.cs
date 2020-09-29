using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class TimeHostedUpdater : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IProgressService _progress;
        private readonly IUpdateInitializer _initializer;

        public TimeHostedUpdater(IProgressService progress, IUpdateInitializer initializer)
        {
            _progress = progress;
            _initializer = initializer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }

        private async void DoWorkAsync(object sender)
        {
            await _initializer.StartUpdatesAsync();
        }

        public Task StopAsync(CancellationToken cancelToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            Dispose();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
