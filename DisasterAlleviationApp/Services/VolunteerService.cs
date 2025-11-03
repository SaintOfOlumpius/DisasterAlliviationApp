using DisasterAlleviationApp.Models;
using MongoDB.Driver;

namespace DisasterAlleviationApp.Services;

public class VolunteerService : IVolunteerService
{
    private readonly IMongoCollection<Volunteer> _volunteers;

    public VolunteerService(IMongoDatabase database)
    {
        _volunteers = database.GetCollection<Volunteer>("Volunteers");
    }

    public async Task<List<Volunteer>> GetAllAsync()
    {
        return await _volunteers.Find(volunteer => true).ToListAsync();
    }

    public async Task<Volunteer?> GetByIdAsync(string id)
    {
        return await _volunteers.Find(volunteer => volunteer.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Volunteer> CreateAsync(Volunteer volunteer)
    {
        await _volunteers.InsertOneAsync(volunteer);
        return volunteer;
    }

    public async Task<bool> UpdateAsync(string id, Volunteer volunteer)
    {
        var result = await _volunteers.ReplaceOneAsync(v => v.Id == id, volunteer);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _volunteers.DeleteOneAsync(v => v.Id == id);
        return result.DeletedCount > 0;
    }
}
