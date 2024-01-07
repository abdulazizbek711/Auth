using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class User
{
    [Key]
    public int User_ID { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public double? WalletBalance { get; set; }
}