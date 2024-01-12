using Auth.Models;

namespace Auth.Interfaces;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    User GetUser(int User_ID);
    bool UserExists(int User_ID);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int User_ID);
}