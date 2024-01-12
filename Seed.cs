using Auth.Data;
using Auth.Models;
using MongoDB.Driver;

namespace Auth;

public class Seed
{
    private readonly IMongoCollection<User> _userCollection;

    public Seed(MongoContext context)
    {
        _userCollection = context.Users;
    }

    public void SeedDataContext()
    {
        if (_userCollection.CountDocuments(FilterDefinition<User>.Empty) == 0)
        {
            var users = new List<User>
            {
                new User { UserName = "User1", Email = "user1@example.com" },
                new User { UserName = "User2", Email = "user2@example.com" },
            };
            _userCollection.InsertMany(users);
        }
    }
}