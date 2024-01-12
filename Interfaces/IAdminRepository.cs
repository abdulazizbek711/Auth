using Auth.Models;

namespace Auth.Interfaces;

public interface IAdminRepository
{
    ICollection<Admin> GetAdmins();
    Admin GetAdmin(int Admin_ID);
    bool AdminExists(int Admin_ID);
    void CreateAdmin(Admin admin);
    void UpdateAdmin(Admin admin);
    void DeleteAdmin(int Admin_ID);
    
}