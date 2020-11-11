using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdateInitializer : IUpdateInitializer
    {
        private readonly IUpdatingProgress _progress;
        private readonly IDataAccessService _dataService;
        private readonly IDataProcessor _dataProcessor;

        public UpdateInitializer(IUpdatingProgress progress, IDataAccessService dataService, IDataProcessor dataProcessor)
        {
            _progress = progress;
            _dataService = dataService;
            _dataProcessor = dataProcessor;
        }

        public async Task StartUpdatesAsync()
        {
            if (!_progress.GetIsUpdatingStarted())
            {
                _progress.SetUpdatingStarted();

                await InitializeWorkDataAsync();

                _progress.SetUpdatingCompleted();
            }
        }

        public void UpdateStatistics()
        {
            _dataService.UpdateDailyStatistics();
        }

        public void VerifyStatisticsForToday()
        {
            if (!_dataService.CheckIfStatsCompleteForToday())
            {
                UpdateStatistics();
            }
        }

        private async Task InitializeWorkDataAsync()
        {
            List<VesselAisUpdateModel> updateList = PopulateUpdateList();
            _dataService.SaveDatabaseQuantities();

            await _dataProcessor.UpdateListOfVesselsAsync(updateList);
        }

        private List<VesselAisUpdateModel> PopulateUpdateList()
        {
            List<VesselAisUpdateModel> list = _dataService.GetVesselsToBeUpdated();
            _progress.SetTotalResultsQuantities(list.Count);

            return list;
        }
    }
}
