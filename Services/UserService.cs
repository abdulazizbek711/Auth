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
    private readonly DataContext _context;

    public UserService(IUserRepository userRepository, DataContext context)
    {
        _userRepository = userRepository;
        _context = context;
    }
       public IEnumerable<User> GetUsers()
    {
        var users = _userRepository.GetUsers();
        if (users == null || !users.Any())
        {
            throw new InvalidOperationException("No users found");
        }
        return users;
    }

       public User GetUser(int User_ID)
       {
           var user = _userRepository.GetUser(User_ID);
           if (user == null)
           {
               throw new InvalidOperationException("No user found");
           }

           return user;
       }

      

       public (bool, string) CreateUser(User user,  UserDto userCreate)
    {
        if (userCreate == null)
        {
            return (false, "No Users Created");
        }
        var existingUser = _userRepository.GetUsers()
            .FirstOrDefault(c => c.UserName != null &&
                                 c.UserName.Trim().ToUpper() == userCreate.UserName.Trim().ToUpper());
        if (existingUser != null)
        {
            return (false, "User already exists");
        }
        _userRepository.CreateUser(user);
        return (true, "User created successfully");
    }
    public User UpdateUser(User user, int User_ID, UserDto updatedUser)
    {
        if (updatedUser == null || User_ID != updatedUser.User_ID)
        {
            return null;
        }

        var existingUser = GetUser(User_ID);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.UserName = updatedUser.UserName ?? existingUser.UserName;
        existingUser.Email = updatedUser.Email ?? existingUser.Email;
        existingUser.WalletBalance = updatedUser.WalletBalance ?? existingUser.WalletBalance;
        // Update the user in the repository
        _userRepository.UpdateUser(existingUser);
        var updatedDbUser = GetUser(User_ID);
        return updatedDbUser;
    }

    public (bool, string) DeleteUser(int User_ID)
    {
        if (!_userRepository.UserExists(User_ID))
        {
            return (false, "User not found");
        }
        var userToDelete = _userRepository.GetUser(User_ID);
        if (userToDelete == null)
        {
            return (false, "User not exist");
        }
        _userRepository.DeleteUser(userToDelete);
        return (true, "User successfully deleted");
    }
}