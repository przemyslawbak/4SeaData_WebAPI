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
        private readonly IScrapper _scrapper;

        public TimeHostedUpdater(IProgressService progress, IScrapper scrapper)
        {
            _progress = progress;
            _scrapper = scrapper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }

        private async void DoWorkAsync(object sender)
        {
            _progress.SetUpdatingStarted();

            await _scrapper.StartUpdatesAsync();

            _progress.SetUpdatingCompleted();
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
