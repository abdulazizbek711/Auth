using System.Collections.Generic;
using System.Linq;
using Auth.Data;
using Auth.Interfaces;
using Auth.Models;
using MongoDB.Driver;

namespace Auth.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(MongoContext context)
        {
            _userCollection = context.Users;
        }

        public ICollection<User> GetUsers()
        {
            return _userCollection.Find(_ => true).ToList();
        }

        public User GetUser(int User_ID)
        {
            return _userCollection.Find(u => u.User_ID == User_ID).FirstOrDefault();
        }

        public bool UserExists(int User_ID)
        {
            return _userCollection.Find(u => u.User_ID == User_ID).Any();
        }

        public void CreateUser(User user)
        {
            _userCollection.InsertOne(user);
        }

        public void UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.User_ID, user.User_ID);
            _userCollection.ReplaceOne(filter, user);
        }

        public void DeleteUser(int User_ID)
        {
            var filter = Builders<User>.Filter.Eq(u => u.User_ID, User_ID);
            _userCollection.DeleteOne(filter);
        }
    }
}