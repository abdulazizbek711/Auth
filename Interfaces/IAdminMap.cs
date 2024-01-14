using Auth.Dto;
using Auth.Models;

namespace Auth.Interfaces;

public interface IAdminMap
{
    public Admin MapAdmin(AdminDto adminCreate);
    public Task<Admin> MappAdmin(int Admin_ID, AdminDto updatedAdmin);
}