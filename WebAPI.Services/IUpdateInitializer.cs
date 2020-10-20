using System.Threading.Tasks;

namespace WebAPI.Services
{
    public interface IUpdateInitializer
    {
        Task StartUpdatesAsync();
        void UpdateStatistics();
    }
}