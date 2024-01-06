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
    public User MappUser(int User_ID, UserDto updatedUser)
    {
        var existingUser = _userRepository.GetUser(User_ID);
        var userMap = _mapper.Map<User>(existingUser);
        var updateResult = _userService.UpdateUser(userMap, User_ID, updatedUser);
        if (!updateResult.Item1)
        {
            throw new InvalidOperationException($"Failed to update user: {updateResult.Item2}");
        }
        return userMap;
    }
}