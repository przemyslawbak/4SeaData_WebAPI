using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class DataProcessor : IDataProcessor
    {
        private readonly IUpdatingProgress _progress;
        private readonly IConfiguration _configuration;
        private readonly IUpdatedVesselFactory _vesselUpdates;
        private readonly IStringParser _stringParser;
        private readonly IDataAccessService _dataService;
        private readonly IExceptionProcessor _exceptionProcessor;

        private int _counter;

        public DataProcessor
            (IUpdatingProgress progress,
            IConfiguration configuration,
            IUpdatedVesselFactory vesselUpdates,
            IStringParser stringParser,
            IDataAccessService dataService,
            IExceptionProcessor exceptionProcessor)
        {
            _progress = progress;
            _configuration = configuration;
            _vesselUpdates = vesselUpdates;
            _stringParser = stringParser;
            _dataService = dataService;
            _exceptionProcessor = exceptionProcessor;
        }

        public async Task<bool> UpdateSingleVesselAsync(int mmsi, int imo, string searchType)
        {
            VesselUpdateModel updatedVessel = await _vesselUpdates.GetVesselUpdatesAsync
                (new VesselAisUpdateModel() { Mmsi = mmsi, Imo = imo, Speed = 0 }, GetCancellationTokenSource().Token, GetSemaphoreThrottel());

            if (updatedVessel != null)
            {
                _dataService.SaveUpdatedVessels(new List<VesselUpdateModel>() { updatedVessel });

                return true;
            }

            return false;
        }

        public async Task UpdateListOfVesselsAsync(List<VesselAisUpdateModel> updateList)
        {
            _counter = 0;

            while (_counter < _progress.GetTotalResultsQuantity())
            {
                await ProcessNextStepAsync(updateList);
            }

            StatusModel progress = _progress.GetProgressStatus();

            _dataService.SaveUpdateLogs(progress);
        }

        private async Task ProcessNextStepAsync(List<VesselAisUpdateModel> updateList)
        {
            List<VesselUpdateModel> updatedVessels = new List<VesselUpdateModel>();

            try
            {
                List<Task> currentRunningTasks = new List<Task>();
                CancellationTokenSource tokenSource = GetCancellationTokenSource();
                SemaphoreSlim semaphoreThrottel = GetSemaphoreThrottel();

                for (int i = _counter; i < _progress.GetCurrentUpdateStep(_counter, _configuration.GetValue<int>("Iteration:Step")); i++)
                {
                    int iteration = i;

                    currentRunningTasks.Add(Task.Run(async () =>
                    {
                        VesselUpdateModel updatedVessel = await _vesselUpdates.GetVesselUpdatesAsync(updateList[iteration], tokenSource.Token, semaphoreThrottel);

                        _progress.UpdateMissingProperties(updatedVessel);
                        _progress.SetLastUpdatedVessel(_stringParser.BuildUpdatedVesselInfo(updatedVessel));

                        lock (((ICollection)updatedVessels).SyncRoot)
                        {
                            updatedVessels.Add(updatedVessel);
                        }

                        _counter++;

                    }, tokenSource.Token));
                }

                await Task.WhenAll(currentRunningTasks);
            }
            catch (Exception ex)
            {
                _counter++;
                _progress.SetLastError(ex.Message + " from: " + _exceptionProcessor.GetMethodNameThrowingException(ex));
            }
            finally
            {
                _progress.SetUpdatingDatabaseTrue();
                SaveUpdatedVessels(updatedVessels);
                _progress.SetUpdatingDatabaseFalse();
            }
        }

        private void SaveUpdatedVessels(List<VesselUpdateModel> updatedVessels)
        {
            _dataService.SaveUpdatedVessels(updatedVessels);
        }

        private SemaphoreSlim GetSemaphoreThrottel()
        {
            return new SemaphoreSlim(_configuration.GetValue<int>("Iteration:ParalelizmDegree"));
        }

        private CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
