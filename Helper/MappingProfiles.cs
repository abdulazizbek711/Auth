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
        CreateMap<Admin, AdminDto>();
        CreateMap<AdminDto, Admin>();
        CreateMap<Admin, AdminnDto>();
        CreateMap<AdminnDto, Admin>();
    }
}