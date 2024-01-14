using Auth.Models;

namespace Auth.Interfaces;

public interface IAdminRepository
{
    public Task<IEnumerable<Admin>> GetAdmins();
    public Task<Admin> GetAdmin(int Admin_ID);
    public Task<bool> AdminExists(int Admin_ID);
    public Task<Admin> CreateAdmin(Admin admin);
    public Task UpdateAdmin(int Admin_ID, Admin admin);
    public Task DeleteAdmin(int Admin_ID);
    
}