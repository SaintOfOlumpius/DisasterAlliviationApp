using DisasterAlleviationApp.Models;

namespace DisasterAlleviationApp.Services;

public interface IBeneficiaryService
{
    Task<List<Beneficiary>> GetAllAsync();
    Task<Beneficiary?> GetByIdAsync(string id);
    Task<Beneficiary> CreateAsync(Beneficiary beneficiary);
    Task<bool> UpdateAsync(string id, Beneficiary beneficiary);
    Task<bool> DeleteAsync(string id);
}
