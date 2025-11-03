using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DisasterAlleviationApp.Models;

public class Beneficiary
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Contact")]
    public string Contact { get; set; } = string.Empty;

    [BsonElement("Address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("ReceivedDonations")]
    public List<string> ReceivedDonations { get; set; } = new();
}
