using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class Scrapper : IScrapper
    {
        private readonly IProgressService _progress;

        public Scrapper(IProgressService progress)
        {
            _progress = progress;
        }

        public async Task StartUpdatesAsync()
        {
            //todo: move entire method to separate service

            for (int i = 0; i < 59; i++)
            {
                while (_progress.GetIsUpdatingPaused())
                {
                    await Task.Delay(100);
                }

                await Task.Delay(1000);

                _progress.SetReturnedVessels(i);

                //todo: collect errors quantity FailedResultsQuantity
                //todo: set errors in LastError
                //todo: verify TotalResultsQuantity
                //todo: ReurnedVesselsInCurrent
                //todo: Finalizing
                //todo: UpdatingDatabase
            }

        }
    }
}
