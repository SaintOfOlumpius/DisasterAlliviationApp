using DisasterAlleviationApp.Models;

namespace DisasterAlleviationApp.Services;

public interface IDisasterService
{
    Task<List<Disaster>> GetAllAsync();
    Task<Disaster?> GetByIdAsync(string id);
    Task<Disaster> CreateAsync(Disaster disaster);
    Task<bool> UpdateAsync(string id, Disaster disaster);
    Task<bool> DeleteAsync(string id);
}
