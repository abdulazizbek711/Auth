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
public class UserController: Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
      [HttpGet]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }
        [HttpPost("{User_ID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var user = _userRepository.GetUsers()
                .Where(c => c.UserName.Trim().ToUpper() == userCreate.UserName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
        [HttpPut("{User_ID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult UpdateUser(int User_ID, [FromBody] UserDto updatedUser)
        {
            if (updatedUser == null)
                return BadRequest(ModelState);
            if (User_ID != updatedUser.User_ID)
                return BadRequest(ModelState);
            if (!_userRepository.UserExists(User_ID))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var userMap = _mapper.Map<User>(updatedUser);
            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{User_ID}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int User_ID)
        {
            if (!_userRepository.UserExists(User_ID))
                return NotFound();
            var userToDelete = _userRepository.GetUser(User_ID);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
            }

            return NoContent();

        }
}