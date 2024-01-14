using System.Security.Cryptography;
using System.Text;
using Auth.Data;
using Auth.Dto;
using Auth.Interfaces;
using Auth.Models;

namespace Auth.Services;

public class AdminService: IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly DapperContext _context;

    public AdminService(IAdminRepository adminRepository, DapperContext context)
    {
        _adminRepository = adminRepository;
        _context = context;
    }
    public async Task<IEnumerable<Admin>> GetAdmins()
    {
        var admins = await _adminRepository.GetAdmins();
        if (admins == null || !admins.Any())
        {
            throw new InvalidOperationException("No admins found");
        }
        return admins;
    }

    public async Task<Admin> GetAdmin(int Admin_ID)
       {
           var admin = await _adminRepository.GetAdmin(Admin_ID);
           if (admin == null)
           {
               throw new InvalidOperationException("No admin found");
           }

           return admin;
       }

      

    public async Task<(bool, string)> CreateAdmin(Admin admin, AdminDto adminCreate)
    {
        try
        {
            if (adminCreate == null)
            {
                return (false, "No Admins Created");
            }

            // Retrieve users asynchronously
            var existingAdmins = await _adminRepository.GetAdmins();

            var existingAdmin = existingAdmins
                .FirstOrDefault(c => c.AdminName != null &&
                                     c.AdminName.Trim().ToUpper() == adminCreate.AdminName.Trim().ToUpper());

            if (existingAdmin != null)
            {
                return (false, "Admin already exists");
            }
            adminCreate.Password = GetHashCode(adminCreate.Admin_ID, adminCreate.Password);

            // Create user asynchronously
            await _adminRepository.CreateAdmin(admin);

            return (true, "Admin created successfully");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            return (false, $"Failed to create admin: {ex.Message}");
        }
    }

    public async Task<Admin> UpdateAdmin(Admin admin, int Admin_ID, AdminDto updatedAdmin)
    {
        try
        {
            if (updatedAdmin == null || Admin_ID != updatedAdmin.Admin_ID)
            {
                return null;
            }

            var existingAdmin = await GetAdmin(Admin_ID);

            if (existingAdmin == null)
            {
                return null;
            }

            existingAdmin.AdminName = updatedAdmin.AdminName ?? existingAdmin.AdminName;
            existingAdmin.Password = updatedAdmin.Password ?? existingAdmin.Password;
            
            if (!string.IsNullOrEmpty(updatedAdmin.Password))
            {
                existingAdmin.Password = GetHashCode(existingAdmin.Admin_ID, updatedAdmin.Password);
            }
            // Update the user in the repository asynchronously
            await _adminRepository.UpdateAdmin(Admin_ID, existingAdmin);

            // Retrieve the updated user from the repository asynchronously
            var updatedDbAdmin = await GetAdmin(Admin_ID);

            return updatedDbAdmin;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            return null;
        }
    }


    public async Task<(bool, string)> DeleteAdmin(int Admin_ID)
    {
        if (!await _adminRepository.AdminExists(Admin_ID))
        {
            return (false, "Admin not found");
        }

        await _adminRepository.DeleteAdmin(Admin_ID);

        return (true, "Admin successfully deleted");
    }

    public string GetHashCode(int Admin_ID, string? password)
    {
        var token = Admin_ID + password;
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(token));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }
            
            return builder.ToString();
        }
    }
}