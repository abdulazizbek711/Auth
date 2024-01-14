using Auth.Data;
using Auth.Dto;
using Auth.Interfaces;
using Auth.Models;
using Auth.Repositories;

namespace Auth.Services;
public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly DapperContext _context;

    public UserService(IUserRepository userRepository, DapperContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }
    public async Task<IEnumerable<User>> GetUsers()
    {
        var users = await _userRepository.GetUsers();
        if (users == null || !users.Any())
        {
            throw new InvalidOperationException("No users found");
        }
        return users;
    }

    public async Task<User> GetUser(int User_ID)
       {
           var user = await _userRepository.GetUser(User_ID);
           if (user == null)
           {
               throw new InvalidOperationException("No user found");
           }

           return user;
       }

    public async Task<(bool, string)> CreateUser(User user, UserDto userCreate)
    {
        try
        {
            if (userCreate == null)
            {
                return (false, "No Users Created");
            }
            
            var existingUsers = await _userRepository.GetUsers();

            var existingUser = existingUsers
                .FirstOrDefault(c => c.UserName != null &&
                                     c.UserName.Trim().ToUpper() == userCreate.UserName.Trim().ToUpper());

            if (existingUser != null)
            {
                return (false, "User already exists");
            }
            
            await _userRepository.CreateUser(user);

            return (true, "User created successfully");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to create user: {ex.Message}");
        }
    }

    public async Task<User> UpdateUser(User user, int User_ID, UserDto updatedUser)
    {
        try
        {
            if (updatedUser == null || User_ID != updatedUser.User_ID)
            {
                return null;
            }

            var existingUser = await GetUser(User_ID);

            if (existingUser == null)
            {
                return null;
            }

            existingUser.UserName = updatedUser.UserName ?? existingUser.UserName;
            existingUser.Email = updatedUser.Email ?? existingUser.Email;
            existingUser.WalletBalance = updatedUser.WalletBalance ?? existingUser.WalletBalance;
            
            await _userRepository.UpdateUser(User_ID, existingUser);
            
            var updatedDbUser = await GetUser(User_ID);

            return updatedDbUser;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<(bool, string)> DeleteUser(int User_ID)
    {
        if (!await _userRepository.UserExists(User_ID))
        {
            return (false, "User not found");
        }

        await _userRepository.DeleteUser(User_ID);

        return (true, "User successfully deleted");
    }

}