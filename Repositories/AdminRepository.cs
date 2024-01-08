using Auth.Data;
using Auth.Interfaces;
using Auth.Models;

namespace Auth.Repositories;

public class AdminRepository: IAdminRepository
{
    private readonly DataContext _context;

    public AdminRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Admin> GetAdmins()
    {
        return _context.Admins.OrderBy(a => a.Admin_ID).ToList();
    }

    public Admin GetAdmin(int Admin_ID)
    {
        return _context.Admins.Where(a => a.Admin_ID==Admin_ID).FirstOrDefault();
    }

    public bool AdminExists(int Admin_ID)
    {
        return _context.Admins.Any(u => u.Admin_ID == Admin_ID);
    }

    public bool CreateAdmin(Admin admin)
    {
        _context.Add(admin);
        return Save();
    }

    public bool UpdateAdmin(Admin admin)
    {
        _context.Update(admin);
        return Save();
    }

    public bool DeleteAdmin(Admin admin)
    {
        _context.Remove(admin);
        return Save();
    }
    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}