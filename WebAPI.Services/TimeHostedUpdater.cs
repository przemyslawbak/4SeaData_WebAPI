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

        public TimeHostedUpdater(IProgressService progress)
        {
            _progress = progress;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!_progress.GetIsUpdatingStarted())
            {
                Task.Run(() =>
                {
                    _timer = new Timer(DoWorkAsync, cancellationToken, TimeSpan.Zero, TimeSpan.FromMinutes(2));

                }, cancellationToken);
            }

            return Task.CompletedTask;
        }

        private async void DoWorkAsync(object sender)
        {
            try
            {
                await StartUpdatesAsync((CancellationToken)sender);
            }
            catch (OperationCanceledException ex)
            {
                //todo:
            }
        }

        private async Task StartUpdatesAsync(CancellationToken cancelToken)
        {
            //todo: move entire method to separate service
            _progress.SetUpdatingStarted();

            for (int i = 1; i < 59; i++)
            {
                while (_progress.GetIsUpdatingPaused())
                {
                    await Task.Delay(100, cancelToken);
                }

                await Task.Delay(1000, cancelToken);

                _progress.SetReturnedVessels(i);

                //todo: collect errors quantity FailedResultsQuantity
                //todo: set errors in LastError
                //todo: verify TotalResultsQuantity
                //todo: ReurnedVesselsInCurrent
                //todo: Finalizing
                //todo: UpdatingDatabase
            }

            _progress.SetUpdatingStopped();
            _progress.SetCompletedUpdatesTime();
        }

        public Task StopAsync(CancellationToken cancelToken)
        {
            _progress.SetUpdatingStopped();

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
