using Auth.Dto;
using Auth.Models;

namespace Auth.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<User>> GetUsers();
    public Task<User> GetUser(int User_ID);
    public Task<(bool, string)> CreateUser(User user, UserDto userCreate);
    public Task<User> UpdateUser(User user, int User_ID, UserDto updatedUser);
    public Task<(bool, string)> DeleteUser(int User_ID);
}