using Auth.Data;
using Auth.Models;
using Dapper;

namespace Auth;

public class Seed
{
    private readonly DapperContext _context;

    public Seed(DapperContext context)
    {
        _context = context;
    }

    public void SeedDataContext()
    {
        // Assuming you have a stored procedure or query to check if data exists
        var count = _context.CreateConnection().QuerySingle<int>("SELECT COUNT(*) FROM Users");

        if (count == 0)
        {
            // Insert sample data
            var users = new List<User>
            {
                new User { UserName = "User1", Email = "user1@example.com" },
                new User { UserName = "User2", Email = "user2@example.com" },
            };

            // Assuming you have a stored procedure or query to insert data
            _context.CreateConnection().Execute("INSERT INTO Users (UserName, Email) VALUES (@UserName, @Email)", users);
        }
    }
}
