using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Models;

public class Admin
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] // Ensure _id is treated as ObjectId
    public string Id { get; set; }

    [Key]
    public int Admin_ID { get; set; }
    public string? AdminName { get; set; }
    public string? Password { get; set; }
    public string? token { get; set; }
}