using WebAPI.Models;

namespace WebAPI.DAL
{
    public interface IADORepository
    {
        VesselAisUpdateModel GetVesselData(string searchQuery);
        void SetUpdates(string v);
    }
}