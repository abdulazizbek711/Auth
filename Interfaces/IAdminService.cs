using Auth.Dto;
using Auth.Models;

namespace Auth.Interfaces;

public interface IAdminService
{
    public Task<IEnumerable<Admin>> GetAdmins();
    public Task<Admin> GetAdmin(int Admin_ID);
    public Task<(bool, string)> CreateAdmin(Admin admin, AdminDto adminCreate);
    public Task<Admin> UpdateAdmin(Admin admin, int Admin_ID, AdminDto updatedAdmin);
    public Task<(bool, string)> DeleteAdmin(int Admin_ID);
}