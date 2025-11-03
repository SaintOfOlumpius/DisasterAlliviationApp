using DisasterAlleviationApp.Models;
using MongoDB.Driver;

namespace DisasterAlleviationApp.Services;

public class DisasterService : IDisasterService
{
    private readonly IMongoCollection<Disaster> _disasters;

    public DisasterService(IMongoDatabase database)
    {
        _disasters = database.GetCollection<Disaster>("Disasters");
    }

    public async Task<List<Disaster>> GetAllAsync()
    {
        return await _disasters.Find(disaster => true).ToListAsync();
    }

    public async Task<Disaster?> GetByIdAsync(string id)
    {
        return await _disasters.Find(disaster => disaster.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Disaster> CreateAsync(Disaster disaster)
    {
        await _disasters.InsertOneAsync(disaster);
        return disaster;
    }

    public async Task<bool> UpdateAsync(string id, Disaster disaster)
    {
        var result = await _disasters.ReplaceOneAsync(d => d.Id == id, disaster);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _disasters.DeleteOneAsync(d => d.Id == id);
        return result.DeletedCount > 0;
    }
}
