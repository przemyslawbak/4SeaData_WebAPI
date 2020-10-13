using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class TimeHostedUpdater : IHostedService, IDisposable
    {
        private Timer _timerUpdates;
        private Timer _timerStatistics;
        private readonly IUpdateInitializer _initializer;

        public TimeHostedUpdater(IUpdateInitializer initializer)
        {
            _initializer = initializer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timerUpdates = new Timer(InitUpdates, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            _timerStatistics = new Timer(InitStatistics, null, TimeSpan.Zero, TimeSpan.FromHours(24));

            return Task.CompletedTask;
        }

        private async void InitUpdates(object sender)
        {
            await _initializer.StartUpdatesAsync();
        }

        private void InitStatistics(object sender)
        {
            _initializer.UpdateStatisticsAsync();
        }

        public Task StopAsync(CancellationToken cancelToken)
        {
            _timerUpdates?.Change(Timeout.Infinite, 0);
            _timerStatistics?.Change(Timeout.Infinite, 0);

            Dispose();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timerUpdates?.Dispose();
        }
    }
}
