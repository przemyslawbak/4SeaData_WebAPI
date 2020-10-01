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
        //todo: too many dependencies, split service
        private readonly IProgressService _progress;
        private readonly IConfiguration _configuration;
        private readonly IVesselFactory _tasksGenerator;
        private readonly IStringParser _stringParser;
        private readonly IDataAccessService _dataService;

        private readonly int _step;
        private readonly int _degreeOfParallelism;
        private readonly List<SeaModel> _seaAreas;
        private int _counter;

        public DataProcessor(IProgressService progress, IConfiguration configuration, IVesselFactory tasksGenerator, IStringParser stringParser, IDataAccessService dataService)
        {
            _progress = progress;
            _configuration = configuration;
            _tasksGenerator = tasksGenerator;
            _stringParser = stringParser;
            _dataService = dataService;

            _step = _configuration.GetValue<int>("Iteration:Step");
            _degreeOfParallelism = _configuration.GetValue<int>("Iteration:ParalelizmDegree");
            _seaAreas = _dataService.GetSeaAreas();
        }

        public async Task IterateThroughUpdateObjectsAsync(List<VesselAisUpdateModel> updateList)
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
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            SemaphoreSlim semaphoreThrottel = new SemaphoreSlim(_degreeOfParallelism);

            for (int i = _counter; i < GetCurrentStep(_counter, _step); i++)
            {
                int iteration = i;

                currentRunningTasks.Add(Task.Run(async () =>
                {
                    VesselUpdateModel updatedVessel = await _tasksGenerator.GetVesselUpdatesAsync(updateList[iteration], _seaAreas, tokenSource.Token, semaphoreThrottel);
                    updatedVessels = AddToList(updatedVessels, updatedVessel);

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
                _dataService.SaveUpdatedVessels(updatedVessels);
            }
        }

        private int GetCurrentStep(int counter, int step)
        {
            if (counter + step > _progress.GetTotalResultsQuantity())
            {
                return _progress.GetTotalResultsQuantity();
            }
            else
            {
                return counter + step;
            }
        }

        private List<VesselUpdateModel> AddToList(List<VesselUpdateModel> updatedVessels, VesselUpdateModel updateVessel)
        {
            _counter++;
            _progress.AddToReturnedResultsQuantity();

            if (updateVessel != null)
            {
                lock (((ICollection)updatedVessels).SyncRoot)
                {
                    updatedVessels.Add(updateVessel);

                    _progress.SetLastUpdatedVessel(_stringParser.BuildUpdatedVesselInfo(updateVessel));
                }
            }

            return updatedVessels;
        }

        public Task<bool> UpdateSingleVesselAsync(int mmsi, int imo, string searchType)
        {
            throw new NotImplementedException(); //todo: implement
        }
    }
}
