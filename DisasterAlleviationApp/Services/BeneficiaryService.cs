using DisasterAlleviationApp.Models;
using MongoDB.Driver;

namespace DisasterAlleviationApp.Services;

public class BeneficiaryService : IBeneficiaryService
{
    private readonly IMongoCollection<Beneficiary> _beneficiaries;

    public BeneficiaryService(IMongoDatabase database)
    {
        _beneficiaries = database.GetCollection<Beneficiary>("Beneficiaries");
    }

    public async Task<List<Beneficiary>> GetAllAsync()
    {
        return await _beneficiaries.Find(beneficiary => true).ToListAsync();
    }

    public async Task<Beneficiary?> GetByIdAsync(string id)
    {
        return await _beneficiaries.Find(beneficiary => beneficiary.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Beneficiary> CreateAsync(Beneficiary beneficiary)
    {
        await _beneficiaries.InsertOneAsync(beneficiary);
        return beneficiary;
    }

    public async Task<bool> UpdateAsync(string id, Beneficiary beneficiary)
    {
        var result = await _beneficiaries.ReplaceOneAsync(b => b.Id == id, beneficiary);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _beneficiaries.DeleteOneAsync(b => b.Id == id);
        return result.DeletedCount > 0;
    }
}
