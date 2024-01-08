using Auth.Dto;
using Auth.Models;

namespace Auth.Interfaces;

public interface IUserService
{
    IEnumerable<User> GetUsers();
    User GetUser(int User_ID);
    public (bool, string) CreateUser(User user, UserDto userCreate);
    User UpdateUser(User user, int User_ID, UserDto updatedUser);
    public (bool, string) DeleteUser(int User_ID);
}