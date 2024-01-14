using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Models;

public class User
{
    [Key]
    [Column("user_id")] // Specify the column name in the database
    public int User_ID { get; set; }

    [Column("username")] // Specify the column name in the database
    public string? UserName { get; set; }

    [Column("email")] // Specify the column name in the database
    public string? Email { get; set; }

    [Column("walletbalance")] // Specify the column name in the database
    public double? WalletBalance { get; set; }
}