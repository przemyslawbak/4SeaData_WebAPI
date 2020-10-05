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

        private readonly int _step;
        private readonly int _degreeOfParallelism;
        private int _counter;

        public DataProcessor(IUpdatingProgress progress, IConfiguration configuration, IUpdatedVesselFactory vesselUpdates, IStringParser stringParser, IDataAccessService dataService)
        {
            _progress = progress;
            _configuration = configuration;
            _vesselUpdates = vesselUpdates;
            _stringParser = stringParser;
            _dataService = dataService;

            _step = _configuration.GetValue<int>("Iteration:Step");
            _degreeOfParallelism = _configuration.GetValue<int>("Iteration:ParalelizmDegree");
        }

        public async Task<bool> UpdateSingleVesselAsync(int mmsi, int imo, string searchType) //todo: unit test
        {
            VesselUpdateModel updatedVessel = await _vesselUpdates.GetVesselUpdatesAsync(new VesselAisUpdateModel() { Mmsi = mmsi, Imo = imo, Speed = 0 }, GetCancellationTokenSource().Token, GetSemaphoreThrottel());

            if (updatedVessel != null)
            {
                _dataService.SaveUpdatedVessel(updatedVessel);

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
        }

        private async Task ProcessNextStepAsync(List<VesselAisUpdateModel> updateList)
        {
            List<VesselUpdateModel> updatedVessels = new List<VesselUpdateModel>();
            List<Task> currentRunningTasks = new List<Task>();
            CancellationTokenSource tokenSource = GetCancellationTokenSource();
            SemaphoreSlim semaphoreThrottel = GetSemaphoreThrottel();

            for (int i = _counter; i < _progress.GetCurrentUpdateStep(_counter, _step); i++)
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

            try
            {
                await Task.WhenAll(currentRunningTasks);
            }
            catch (Exception ex)
            {
                _progress.SetLastError(ex.Message);
            }
            finally
            {
                SaveUpdatedVessels(updatedVessels);
            }
        }

        private void SaveUpdatedVessels(List<VesselUpdateModel> updatedVessels)
        {
            foreach (var vessel in updatedVessels)
            {
                _dataService.SaveUpdatedVessel(vessel);
            }
        }

        private SemaphoreSlim GetSemaphoreThrottel()
        {
            return new SemaphoreSlim(_degreeOfParallelism);
        }

        private CancellationTokenSource GetCancellationTokenSource()
        {
            return new CancellationTokenSource();
        }
    }
}
