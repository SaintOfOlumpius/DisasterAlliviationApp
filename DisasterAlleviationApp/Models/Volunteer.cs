using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DisasterAlleviationApp.Models;

public class Volunteer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Contact")]
    public string Contact { get; set; } = string.Empty;

    [BsonElement("AssignedDisasters")]
    public List<string> AssignedDisasters { get; set; } = new();
}
