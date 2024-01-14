using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Models;

public class Admin
{
    [Key]
    [Column("admin_id")]
    public int Admin_ID { get; set; }
    [Column("adminname")]
    public string? AdminName { get; set; }
    [Column("password")]
    public string? Password { get; set; }
    [Column("token")]
    public string? token { get; set; }
}