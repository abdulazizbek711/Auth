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
    private readonly DapperContext _context;

    public AdminController(IAdminRepository adminRepository, IMapper mapper, IAdminService adminService, IAdminMap adminMap, DapperContext context, AdminnDto adminnDto)
    {
        _adminRepository = adminRepository;
        _mapper = mapper;
        _adminService = adminService;
        _adminMap = adminMap;
        _context = context;
    }
     [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Admin>))]
        public async Task<IActionResult> GetAdmins()
        {
            var admins = await _adminService.GetAdmins();

            var adminDtos = admins.Select(admin => new AdminnDto
            {
                Admin_ID = admin.Admin_ID,
                token = admin.Password // Note: This might not be secure; consider using a secure token generation method
            });

            return Ok(adminDtos);
        }

        [HttpPost("{Admin_ID}")]
        [ProducesResponseType(201, Type = typeof(AdminDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAdmin(AdminDto adminCreate)
        {
            var adminMap = _adminMap.MapAdmin(adminCreate);
            (bool success, string message) result = await _adminService.CreateAdmin(adminMap, adminCreate);

            if (result.success)
            {
                return CreatedAtAction(nameof(GetAdmins), new { Admin_ID = adminMap.Admin_ID }, adminCreate);
            }

            ModelState.AddModelError("", result.message);
            return BadRequest(ModelState);
        }

        [HttpPut("{Admin_ID}")]
        [ProducesResponseType(200, Type = typeof(AdminDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAdmin(int Admin_ID, [FromBody] AdminDto updatedAdmin)
        {
            var adminMap = await _adminMap.MappAdmin(Admin_ID, updatedAdmin);
            var updatedDbAdmin = _adminService.UpdateAdmin(adminMap, Admin_ID, updatedAdmin);

            if (updatedDbAdmin == null)
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return Ok(updatedDbAdmin);
        }

        [HttpDelete("{Admin_ID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAdmin(int Admin_ID)
        {
            var adminToDelete = _adminRepository.GetAdmin(Admin_ID);
            (bool success, string message) result = await _adminService.DeleteAdmin(Admin_ID);

            if (!result.success)
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    
        }

