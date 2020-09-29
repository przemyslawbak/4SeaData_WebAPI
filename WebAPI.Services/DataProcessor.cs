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
        private readonly ITaskGenerator _tasksGenerator;
        private readonly IStringParser _stringParser;
        private readonly IDataAccessService _dataService;

        private readonly int _step;
        private readonly int _degreeOfParallelism;
        private int _counter;

        public DataProcessor(IProgressService progress, IConfiguration configuration, ITaskGenerator tasksGenerator, IStringParser stringParser, IDataAccessService dataService)
        {
            _progress = progress;
            _configuration = configuration;
            _tasksGenerator = tasksGenerator;
            _stringParser = stringParser;
            _dataService = dataService;

            _step = _configuration.GetValue<int>("Iteration:Step");
            _degreeOfParallelism = _configuration.GetValue<int>("Iteration:ParalelizmDegree");
        }

        public async Task IterateThroughObjects(List<VesselAisUpdateModel> updateList, List<SeaModel> seaAreas)
        {
            _counter = 0;

            while (_counter < _progress.GetTotalResultsQuantity())
            {
                await ProcessNextStep(updateList, seaAreas);
            }
        }

        private async Task ProcessNextStep(List<VesselAisUpdateModel> updateList, List<SeaModel> seaAreas)
        {
            int currentStep = GetCurrentStep(_counter, _step);
            List<VesselModel> updatedVessels = new List<VesselModel>();
            List<Task<VesselModel>> currentRunningTasks = new List<Task<VesselModel>>();
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            SemaphoreSlim semaphoreThrottel = new SemaphoreSlim(_degreeOfParallelism);

            for (int i = _counter; i < currentStep; i++)
            {
                int iteration = i;

                Task<VesselModel> updateVesselTask = _tasksGenerator.CreateNewTask(updateList[iteration], seaAreas, tokenSource.Token, semaphoreThrottel);
                currentRunningTasks.Add(updateVesselTask);
                updatedVessels = AddToList(updatedVessels, updateVesselTask);
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

        private List<VesselModel> AddToList(List<VesselModel> updatedVessels, Task<VesselModel> updateVesselTask)
        {
            _counter++;
            _progress.AddToReturnedResultsQuantity();

            if (updateVesselTask.Result != null)
            {
                lock (((ICollection)updatedVessels).SyncRoot)
                {
                    updatedVessels.Add(updateVesselTask.Result);

                    _progress.SetLastUpdatedVessel(_stringParser.BuildUpdatedVesselInfo(updateVesselTask.Result));
                }
            }

            return updatedVessels;
        }
    }
}
