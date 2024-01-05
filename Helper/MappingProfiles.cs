using Auth.Dto;
using Auth.Models;
using AutoMapper;

namespace Auth.Helper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}