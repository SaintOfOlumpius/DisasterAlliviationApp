using DisasterAlleviationApp.Models;
using MongoDB.Driver;

namespace DisasterAlleviationApp.Services;

public class DonationService : IDonationService
{
    private readonly IMongoCollection<Donation> _donations;

    public DonationService(IMongoDatabase database)
    {
        _donations = database.GetCollection<Donation>("Donations");
    }

    public async Task<List<Donation>> GetAllAsync()
    {
        return await _donations.Find(donation => true).ToListAsync();
    }

    public async Task<Donation?> GetByIdAsync(string id)
    {
        return await _donations.Find(donation => donation.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Donation> CreateAsync(Donation donation)
    {
        await _donations.InsertOneAsync(donation);
        return donation;
    }

    public async Task<bool> UpdateAsync(string id, Donation donation)
    {
        var result = await _donations.ReplaceOneAsync(d => d.Id == id, donation);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _donations.DeleteOneAsync(d => d.Id == id);
        return result.DeletedCount > 0;
    }
}
