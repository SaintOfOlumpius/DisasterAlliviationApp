using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DisasterAlleviationApp.Models;

public class Disaster
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("DateOccurred")]
    public DateTime DateOccurred { get; set; }

    [BsonElement("Location")]
    public string Location { get; set; } = string.Empty;
}
