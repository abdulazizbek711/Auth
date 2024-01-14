using System.Collections.Generic;
using System.Data;
using System.Linq;
using Auth.Data;
using Auth.Interfaces;
using Auth.Models;
using Dapper;
using MongoDB.Driver;

namespace Auth.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DapperContext _context;


        public AdminRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Admin>> GetAdmins()
        {
            var query = "SELECT * FROM admins";
            using (var connection = _context.CreateConnection())
            {
                var admins = await connection.QueryAsync<Admin>(query);
                return admins.ToList();
            }
        }

        public async Task<Admin> GetAdmin(int Admin_ID)
        {
            var query = "SELECT * FROM admins WHERE admin_id = @Admin_ID";
            using (var connection = _context.CreateConnection())
            {
                var admin = await connection.QueryFirstOrDefaultAsync<Admin>(query, new { Admin_ID });
                return admin;
            }
        }

        public async Task<bool> AdminExists(int Admin_ID)
        {
            var query = "SELECT 1 FROM admins WHERE admin_id = @Admin_ID LIMIT 1";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<int>(query, new { Admin_ID });
                return result == 1;
            }
        }


        public async Task<Admin> CreateAdmin(Admin admin)
        {
            var insertQuery = "INSERT INTO admins (admin_id, adminname, password, token) VALUES (@Admin_ID, @AdminName, @Password, @token)";
            var selectQuery = "SELECT * FROM admins WHERE admin_id = @Admin_ID";
            var parameters = new DynamicParameters();
            parameters.Add("Admin_ID", admin.Admin_ID, DbType.Int32);
            parameters.Add("AdminName", admin.AdminName, DbType.String);
            parameters.Add("Password", admin.Password, DbType.String);
            parameters.Add("token", admin.token, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                // Execute the INSERT query
                await connection.ExecuteAsync(insertQuery, parameters);

                // Execute the SELECT query to retrieve the newly created user
                var createdAdmin = await connection.QueryFirstOrDefaultAsync<Admin>(selectQuery, new { Admin_ID = admin.Admin_ID });
        
                return createdAdmin;
            }
        }

        public async Task UpdateAdmin(int Admin_ID, Admin admin)
        {
            var query = "UPDATE admins SET adminname = @AdminName, password = @Password, token = @token WHERE admin_id = @Admin_ID";
            var parameters = new DynamicParameters();
            parameters.Add("Admin_ID", Admin_ID, DbType.Int32);
            parameters.Add("AdminName", admin.AdminName, DbType.String);
            parameters.Add("Password", admin.Password, DbType.String);
            parameters.Add("token", admin.token, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteAdmin(int Admin_ID)
        {
            var query = "DELETE FROM admins WHERE admin_id = @Admin_ID";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Admin_ID });
            }
        }
    }
}