using Auth.Dto;
using Auth.Interfaces;
using Auth.Models;
using AutoMapper;

namespace Auth.Helper;

public class AdminMap: IAdminMap
{
    private readonly IMapper _mapper;
    private readonly IAdminService _adminService;
    private readonly IAdminRepository _adminRepository;

    public AdminMap(IMapper mapper, IAdminService adminService, IAdminRepository adminRepository)
    {
        _mapper = mapper;
        _adminService = adminService;
        _adminRepository = adminRepository;
    }
    public Admin MapAdmin(AdminDto adminCreate)
    {
        var adminMap = _mapper.Map<Admin>(adminCreate);
        if (adminMap == null)
        {
            throw new InvalidOperationException("Something went wrong while saving");
        }
        return adminMap;
    }
    public async Task<Admin> MappAdmin(int Admin_ID, AdminDto updatedAdmin)
    {
        var existingAdmin = await _adminRepository.GetAdmin(Admin_ID);
        if(existingAdmin != null)
        {
            var adminMap = _mapper.Map<Admin>(existingAdmin);
            var updateResult = await _adminService.UpdateAdmin(adminMap, Admin_ID, updatedAdmin);
            return adminMap;
        }
    
        // Handle the case where the user is not found (you might want to throw an exception or return null)
        return null;
    }
}