using Auth.Dto;
using Auth.Interfaces;
using Auth.Models;
using AutoMapper;

namespace Auth.Helper;

public class UserMap: IUserMap
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public UserMap(IMapper mapper, IUserService userService, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userService = userService;
        _userRepository = userRepository;
    }
    public User MapUser(UserDto userCreate)
    {
        var userMap = _mapper.Map<User>(userCreate);
        if (userMap == null)
        {
            throw new InvalidOperationException("Something went wrong while saving");
        }
        return userMap;
    }
    public async Task<User> MappUser(int User_ID, UserDto updatedUser)
    {
        // Ensure to use await here
        var existingUser = await _userRepository.GetUser(User_ID);
        
        if(existingUser != null)
        {
            var userMap = _mapper.Map<User>(existingUser);
            var updateResult = await _userService.UpdateUser(userMap, User_ID, updatedUser);
            return userMap;
        }
        return null;
    }
}