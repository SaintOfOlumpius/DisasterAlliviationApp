using DisasterAlleviationApp.Models;

namespace DisasterAlleviationApp.Services;

public interface IDonationService
{
    Task<List<Donation>> GetAllAsync();
    Task<Donation?> GetByIdAsync(string id);
    Task<Donation> CreateAsync(Donation donation);
    Task<bool> UpdateAsync(string id, Donation donation);
    Task<bool> DeleteAsync(string id);
}
