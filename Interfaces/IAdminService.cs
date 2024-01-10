using Auth.Dto;
using Auth.Models;

namespace Auth.Interfaces;

public interface IAdminService
{
    IEnumerable<Admin> GetAdmins();
    Admin GetAdmin(int Admin_ID);
    public (bool, string) CreateAdmin(AdminDto adminCreate);
    Admin UpdateAdmin(Admin admin, int Admin_ID, AdminDto updatedAdmin);
    public (bool, string) DeleteAdmin(int Admin_ID);
    public string GetHashCode(int Admin_ID, string? password);
}