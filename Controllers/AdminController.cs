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
    private readonly MongoContext _context;

    public AdminController(IAdminRepository adminRepository, IMapper mapper, IAdminService adminService, IAdminMap adminMap, MongoContext context, AdminnDto adminnDto)
    {
        _adminRepository = adminRepository;
        _mapper = mapper;
        _adminService = adminService;
        _adminMap = adminMap;
        _context = context;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<AdminnDto>))]
    public IActionResult GetAdmins()
    {
        var admins = _adminService.GetAdmins();

        var adminDtos = admins.Select(admin => new AdminnDto
        {
            Admin_ID = admin.Admin_ID,
            token = admin.Password 
        });

        return Ok(adminDtos);
    }



    [HttpPost("{Admin_ID}")]
    [ProducesResponseType(201, Type = typeof(AdminnDto))]
    [ProducesResponseType(400)]
    public IActionResult CreateUser(AdminDto adminCreate)
    {
        var result = _adminService.CreateAdmin(adminCreate);

        if (result.Item1)
        {
            string hashedToken = _adminService.GetHashCode(adminCreate.Admin_ID, adminCreate.Password);

            AdminnDto responseDto = new AdminnDto
            {
                Admin_ID = adminCreate.Admin_ID,
                token = hashedToken
            };

            return CreatedAtAction(nameof(GetAdmins), new { Admin_ID = responseDto.Admin_ID }, responseDto);
        }

        return BadRequest(result.Item2);
    }


        [HttpPut("{Admin_ID}")]
        [ProducesResponseType(200, Type = typeof(AdminnDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAdmin(int Admin_ID, [FromBody] AdminDto updatedAdmin)
        {
            var adminMap = _adminMap.MappAdmin(Admin_ID, updatedAdmin);
            var existingAdmin = _adminService.UpdateAdmin(adminMap, Admin_ID, updatedAdmin);

            if (existingAdmin != null)
            {
                string hashedToken = _adminService.GetHashCode(Admin_ID, updatedAdmin.Password);
                
                AdminnDto responseDto = new AdminnDto
                {
                    Admin_ID = Admin_ID,
                    token = hashedToken
                };

                return Ok(responseDto);
            }

            return NotFound();
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

