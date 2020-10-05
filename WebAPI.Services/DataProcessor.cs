﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    //todo: unit testing
    public class DataProcessor : IDataProcessor
    {
        //todo: too many dependencies, split service
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
                    VesselUpdateModel updatedVessel = await _vesselUpdates.GetVesselUpdatesAsync(updateList[iteration], tokenSource.Token, semaphoreThrottel);
                    if (updatedVessel != null)
                    {
                        _progress.UpdateMissingProperties(updatedVessel);
                        updatedVessels = AddToList(updatedVessels, updatedVessel);
                    }

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
