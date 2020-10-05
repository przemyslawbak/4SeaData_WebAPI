﻿using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class TimeHostedUpdater : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IUpdateInitializer _initializer;

        public TimeHostedUpdater(IUpdateInitializer initializer)
        {
            _initializer = initializer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(InitUpdates, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }

        private async void InitUpdates(object sender)
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
