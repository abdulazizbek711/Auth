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
    private readonly MongoContext _context;

    public AdminService(IAdminRepository adminRepository, MongoContext context)
    {
        _adminRepository = adminRepository;
        _context = context;
    }
    public IEnumerable<Admin> GetAdmins()
    {
        var admins = _adminRepository.GetAdmins();
        if (admins == null || !admins.Any())
        {
            throw new InvalidOperationException("No admins found");
        }
        return admins;
    }

       public Admin GetAdmin(int Admin_ID)
       {
           var admin = _adminRepository.GetAdmin(Admin_ID);
           if (admin == null)
           {
               throw new InvalidOperationException("No user found");
           }

           return admin;
       }



       public (bool Success, string Message) CreateAdmin(AdminDto adminCreate)
       {
           if (adminCreate == null)
           {
               return (false, "No Admins Created");
           }

           // Hash the password
           adminCreate.Password = GetHashCode(adminCreate.Admin_ID, adminCreate.Password);

           // Check if an admin with the specified ID already exists
           var existingAdmin = _adminRepository.GetAdmins()
               .FirstOrDefault(c => c.Admin_ID != null &&
                                    c.Admin_ID.ToString().Trim().ToUpper() ==
                                    adminCreate.Admin_ID.ToString().Trim().ToUpper());

           if (existingAdmin != null)
           {
               return (false, "Admin already exists");
           }

           // Create a new Admin entity
           var newAdmin = new Admin
           {
               Admin_ID = adminCreate.Admin_ID,
               AdminName = adminCreate.AdminName,
               Password = adminCreate.Password
               // Set other properties as needed
           };

           try
           {
               // Save the new admin to the data store
               _adminRepository.CreateAdmin(newAdmin);
               return (true, "Admin created successfully");
           }
           catch (Exception ex)
           {
               // Log or handle the exception as needed
               return (false, $"Failed to create admin: {ex.Message}");
           }
       }

       public Admin UpdateAdmin(Admin admin, int Admin_ID, AdminDto updatedAdmin)
       {
           if (updatedAdmin == null || Admin_ID != updatedAdmin.Admin_ID)
           {
               return null;
           }

           var existingAdmin = GetAdmin(Admin_ID);

           if (existingAdmin == null)
           {
               return null;
           }

           // Update the properties of the existing admin
           existingAdmin.AdminName = updatedAdmin.AdminName;

           // Hash the updated password if provided
           if (!string.IsNullOrEmpty(updatedAdmin.Password))
           {
               existingAdmin.Password = GetHashCode(existingAdmin.Admin_ID, updatedAdmin.Password);
           }

           // Update the admin in the repository
           _adminRepository.UpdateAdmin(existingAdmin);

           // Return the updated admin
           return existingAdmin;
       }



    public (bool, string) DeleteAdmin(int Admin_ID)
    {
        if (!_adminRepository.AdminExists(Admin_ID))
        {
            return (false, "Admin not found");
        }
        var adminToDelete = _adminRepository.GetAdmin(Admin_ID);
        if (adminToDelete == null)
        {
            return (false, "Admin not exist");
        }
        _adminRepository.DeleteAdmin(Admin_ID);
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