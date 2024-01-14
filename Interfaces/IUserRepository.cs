using Auth.Models;

namespace Auth.Interfaces;

public interface IUserRepository
{
    public Task<IEnumerable<User>> GetUsers();
    public Task<User> GetUser(int User_ID);
    public Task<bool> UserExists(int User_ID);
    public Task<User> CreateUser(User user);
    public Task UpdateUser(int User_ID, User user);
    public Task DeleteUser(int User_ID);
}