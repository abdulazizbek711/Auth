using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Models;

public class User
{
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] 
    public string Id { get; set; }
    public int User_ID { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public double? WalletBalance { get; set; }
}