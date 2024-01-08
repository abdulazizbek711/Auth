using Auth.Models;

namespace Auth.Interfaces;

public interface IAdminRepository
{
    ICollection<Admin> GetAdmins();
    Admin GetAdmin(int Admin_ID);
    bool AdminExists(int Admin_ID);
    bool CreateAdmin(Admin admin);
    bool UpdateAdmin(Admin admin);
    bool DeleteAdmin(Admin admin);
}