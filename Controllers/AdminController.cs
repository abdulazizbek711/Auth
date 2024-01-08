using Auth.Data;
using Auth.Dto;
using Auth.Interfaces;
using Auth.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController: Controller
{
    private readonly IAdminRepository _adminRepository;
    private readonly IMapper _mapper;
    private readonly IAdminService _adminService;
    private readonly IAdminMap _adminMap;
    private readonly DataContext _context;
    private readonly AdminnDto _adminnDto;

    public AdminController(IAdminRepository adminRepository, IMapper mapper, IAdminService adminService, IAdminMap adminMap, DataContext context, AdminnDto adminnDto)
    {
        _adminRepository = adminRepository;
        _mapper = mapper;
        _adminService = adminService;
        _adminMap = adminMap;
        _context = context;
        _adminnDto = adminnDto;
    }
    [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Admin>))]
        public IActionResult GetAdmins()
        {
            var admins = _adminService.GetAdmins();
            return Ok(admins);
        }
        [HttpPost("{Admin_ID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser(AdminDto adminCreate)
        {
            var adminMap = _adminMap.MapAdmin(adminCreate);
            (bool success, string message) result = _adminService.CreateAdmin(adminMap, adminCreate);
            if (!result.success)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return BadRequest(ModelState);
            }
            return Ok(_adminnDto);
        }
        [HttpPut("{Admin_ID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAdmin(int Admin_ID, [FromBody] AdminDto updatedAdmin)
        {
            var adminMap = _adminMap.MappAdmin(Admin_ID, updatedAdmin);
            var updatedDbAdmin = _adminService.UpdateAdmin(adminMap, Admin_ID, updatedAdmin);

            if (updatedDbAdmin == null)
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return Ok(_adminnDto);
        }
    
        [HttpDelete("{Admin_ID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAdmin(int Admin_ID)
        {
            var adminToDelete = _adminRepository.GetAdmin(Admin_ID);
            (bool success, string message) result = _adminService.DeleteAdmin(Admin_ID);
            if (!result.success)
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }
            return NoContent();
        }
}