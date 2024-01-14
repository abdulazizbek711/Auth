using System.Security.Claims;
using Auth.Data;
using Auth.Dto;
using Auth.Interfaces;
using Auth.Models;
using Auth.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UserController: ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IUserMap _userMap;
    private readonly DapperContext _context;

    public UserController(IUserRepository userRepository, IMapper mapper, IUserService userService, IUserMap userMap)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userService = userService;
        _userMap = userMap;
    }
    [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users.Result);
        }
        [HttpPost("{User_ID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateUser(UserDto userCreate)
        {
            var userMap = _userMap.MapUser(userCreate);
            (bool success, string message) result = await _userService.CreateUser(userMap, userCreate);

            if (result.success)
            {
                return CreatedAtAction(nameof(GetUsers), new { User_ID = userMap.User_ID }, userCreate);
            }

            ModelState.AddModelError("", result.message);
            return BadRequest(ModelState);
        }
        [HttpPut("{User_ID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async  Task<IActionResult> UpdateUser(int User_ID, [FromBody] UserDto updatedUser)
        {
            var userMap =  await _userMap.MappUser(User_ID, updatedUser);
            var updatedDbUser = _userService.UpdateUser(userMap, User_ID, updatedUser);

            if (updatedDbUser == null)
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return BadRequest(ModelState);
            }

            return Ok(updatedDbUser.Result);
        }
    
        [HttpDelete("{User_ID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int User_ID)
        {
            var userToDelete = _userRepository.GetUser(User_ID);
            (bool success, string message) result = await _userService.DeleteUser(User_ID);

            if (!result.success)
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
}