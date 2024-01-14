using Auth.Data;
using Auth.Dto;
using Auth.Interfaces;
using Auth.Models;
using Auth.Repositories;

namespace Auth.Services;
//Need to do exceptions 400, 404 for all conditions==>>
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

            // Retrieve users asynchronously
            var existingUsers = await _userRepository.GetUsers();

            var existingUser = existingUsers
                .FirstOrDefault(c => c.UserName != null &&
                                     c.UserName.Trim().ToUpper() == userCreate.UserName.Trim().ToUpper());

            if (existingUser != null)
            {
                return (false, "User already exists");
            }

            // Create user asynchronously
            await _userRepository.CreateUser(user);

            return (true, "User created successfully");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
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

            // Update the user in the repository asynchronously
            await _userRepository.UpdateUser(User_ID, existingUser);

            // Retrieve the updated user from the repository asynchronously
            var updatedDbUser = await GetUser(User_ID);

            return updatedDbUser;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
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