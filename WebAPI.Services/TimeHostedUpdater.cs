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
        private DateTime _nextDay = DateTime.UtcNow.AddDays(1);

        public TimeHostedUpdater(IUpdateInitializer initializer)
        {
            _initializer = initializer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timerUpdates = new Timer(InitUpdates, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            _timerStatistics = new Timer(InitStatistics, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void InitUpdates(object sender)
        {
            await _initializer.StartUpdatesAsync();
        }

        private void InitStatistics(object sender)
        {
            if (DateTime.UtcNow.Day == _nextDay.Day)
            {
                _nextDay = DateTime.UtcNow.AddDays(1);

                _initializer.UpdateStatistics();
            }
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
