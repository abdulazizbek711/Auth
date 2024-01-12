using System.Collections.Generic;
using System.Linq;
using Auth.Data;
using Auth.Interfaces;
using Auth.Models;
using MongoDB.Driver;

namespace Auth.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IMongoCollection<Admin> _adminCollection;

        public AdminRepository(MongoContext context)
        {
            _adminCollection = context.Admins;
        }

        public ICollection<Admin> GetAdmins()
        {
            return _adminCollection.Find(_ => true).ToList();
        }

        public Admin GetAdmin(int adminId)
        {
            return _adminCollection.Find(a => a.Admin_ID == adminId).FirstOrDefault();
        }

        public bool AdminExists(int Admin_ID)
        {
            return _adminCollection.Find(u => u.Admin_ID == Admin_ID).Any();
        }

        public void CreateAdmin(Admin admin)
        {
            _adminCollection.InsertOne(admin);
        }

        public void UpdateAdmin(Admin admin)
        {
            var filter = Builders<Admin>.Filter.Eq(a => a.Admin_ID, admin.Admin_ID);
            _adminCollection.ReplaceOne(filter, admin);
        }

        public void DeleteAdmin(int Admin_ID)
        {
            var filter = Builders<Admin>.Filter.Eq(a => a.Admin_ID, Admin_ID);
            _adminCollection.DeleteOne(filter);
        }
    }
}