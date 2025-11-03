using DisasterAlleviationApp.Models;

namespace DisasterAlleviationApp.Services;

public interface IVolunteerService
{
    Task<List<Volunteer>> GetAllAsync();
    Task<Volunteer?> GetByIdAsync(string id);
    Task<Volunteer> CreateAsync(Volunteer volunteer);
    Task<bool> UpdateAsync(string id, Volunteer volunteer);
    Task<bool> DeleteAsync(string id);
}
