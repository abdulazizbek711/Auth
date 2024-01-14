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
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;


        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = "SELECT * FROM users";
            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<User>(query);
                return users.ToList();
            }
        }

        public async Task<User> GetUser(int User_ID)
        {
            var query = "SELECT * FROM users WHERE user_ID = @User_ID";
            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(query, new { User_ID });
                return user;
            }
        }

        public async Task<bool> UserExists(int User_ID)
        {
            var query = "SELECT 1 FROM users WHERE user_id = @User_ID LIMIT 1";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<int>(query, new { User_ID });
                return result == 1;
            }
        }


        public async Task<User> CreateUser(User user)
        {
            var insertQuery = "INSERT INTO users (user_id, username, email, walletbalance) VALUES (@User_ID, @UserName, @Email, @WalletBalance)";
            var selectQuery = "SELECT * FROM users WHERE user_id = @User_ID";

            var parameters = new DynamicParameters();
            parameters.Add("User_ID", user.User_ID, DbType.Int32);
            parameters.Add("UserName", user.UserName, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("WalletBalance", user.WalletBalance, DbType.Double); // Use DbType.Double for double?

            using (var connection = _context.CreateConnection())
            {
                // Execute the INSERT query
                await connection.ExecuteAsync(insertQuery, parameters);

                // Execute the SELECT query to retrieve the newly created user
                var createdUser = await connection.QueryFirstOrDefaultAsync<User>(selectQuery, new { User_ID = user.User_ID });
        
                return createdUser;
            }
        }


        public async Task UpdateUser(int User_ID, User user)
        {
            var query = "UPDATE users SET username = @UserName, email = @Email, walletbalance = @WalletBalance WHERE user_id = @User_ID";
            var parameters = new DynamicParameters();
            parameters.Add("User_ID", User_ID, DbType.Int32);
            parameters.Add("UserName", user.UserName, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("WalletBalance", user.WalletBalance, DbType.Double);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteUser(int User_ID)
        {
            var query = "DELETE FROM users WHERE user_id = @User_ID";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { User_ID });
            }
        }
    }
}