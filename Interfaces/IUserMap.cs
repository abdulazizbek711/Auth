using Auth.Dto;
using Auth.Models;

namespace Auth.Interfaces;

public interface IUserMap
{
    public User MapUser(UserDto userCreate);
    public Task<User> MappUser(int User_ID, UserDto updatedUser);
}