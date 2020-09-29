using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UpdateInitializer : IUpdateInitializer
    {
        private readonly IProgressService _progress;
        private readonly IDataAccessService _dataService;
        private readonly IDataProcessor _dataProcessor;

        public UpdateInitializer(IProgressService progress, IDataAccessService dataService, IDataProcessor dataProcessor)
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

                await InitializeWorkData();

                _progress.SetUpdatingCompleted();
            }
        }

        private async Task InitializeWorkData()
        {
            List<VesselAisUpdateModel> updateList = PopulateUpdateList();
            List<SeaModel> seaAreas = PopulateSeaAreaList();
            _dataService.UpdateDataQuantities();

            await _dataProcessor.IterateThroughObjects(updateList, seaAreas);
        }

        private List<SeaModel> PopulateSeaAreaList()
        {
            return _dataService.GetSeaAreas();
        }

        private List<VesselAisUpdateModel> PopulateUpdateList()
        {
            List<VesselAisUpdateModel> list = _dataService.GetVesselsToBeUpdated();
            _progress.SetTotalResultsQuantities(list.Count);

            return list;
        }
    }
}
