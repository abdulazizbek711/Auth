using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class Admin
{
    [Key]
    public int Admin_ID { get; set; }
    public string? AdminName { get; set; }
    public string? Password { get; set; }
    public string? token { get; set; }
}