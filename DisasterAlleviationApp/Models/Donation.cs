using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DisasterAlleviationApp.Models;

public class Donation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("Description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("Amount")]
    public decimal Amount { get; set; }

    [BsonElement("Date")]
    public DateTime Date { get; set; }
}
