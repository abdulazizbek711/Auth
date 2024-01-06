using Auth.Dto;
using Auth.Models;

namespace Auth.Interfaces;

public interface IUserService
{
    IEnumerable<User> GetUsers();
    public (bool, string) CreateUser(User user, UserDto userCreate);
    public (bool, string) UpdateUser(User user, int User_ID, UserDto updatedUser);
    public (bool, string) DeleteUser(int User_ID);
}