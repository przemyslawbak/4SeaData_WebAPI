using System.Threading.Tasks;

namespace WebAPI.Services
{
    public interface IScrapper
    {
        Task StartUpdatesAsync();
    }
}