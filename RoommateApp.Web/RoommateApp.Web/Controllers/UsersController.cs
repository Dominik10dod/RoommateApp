using Microsoft.AspNetCore.Mvc;
using RoommateApp.Bll.DTOs;
using RoommateApp.Bll.Services;

namespace RoommateApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)
        {
            var user = await _userService.RegisterAsync(dto);
            if (user == null)
                return BadRequest("Ez az e-mail cím már regisztrálva van!");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.LoginAsync(dto);
            if (user == null)
                return Unauthorized("Hibás e-mail cím vagy jelszó!");

            return Ok(user);
        }
    }
}