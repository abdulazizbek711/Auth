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
    public Admin MappAdmin(int Admin_ID, AdminDto updatedAdmin)
    {
        var existingAdmin = _adminRepository.GetAdmin(Admin_ID);
        var adminMap = _mapper.Map<Admin>(existingAdmin);
        var updateResult = _adminService.UpdateAdmin(adminMap, Admin_ID, updatedAdmin);
        return adminMap;
    }
}