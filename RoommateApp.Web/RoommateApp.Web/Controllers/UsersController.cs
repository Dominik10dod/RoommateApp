using Microsoft.AspNetCore.Mvc;
using RoommateApp.Bll.DTOs;
using RoommateApp.Bll.Services;
using Google.Apis.Auth;
using RoommateApp.Bll.DTOs;

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

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto dto)
        {
            var user = await _userService.LoginAsync(dto);
            if (user == null) return Unauthorized();
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            var user = await _userService.RegisterAsync(dto);
            return Ok(user);
        }

        [HttpGet("apartment/{apartmentId}")]
        public async Task<ActionResult<List<UserDto>>> GetUsersByApartment(int apartmentId)
        {
            var users = await _userService.GetUsersByApartmentAsync(apartmentId);
            return Ok(users);
        }

        [HttpGet("{userId}/apartments")]
        public async Task<ActionResult<List<ApartmentDto>>> GetUserApartments(int userId)
        {
            var apartments = await _userService.GetUserApartmentsAsync(userId);
            return Ok(apartments);
        }

        [HttpPost("{userId}/create-apartment")]
        public async Task<ActionResult<ApartmentDto>> CreateApartment(int userId, [FromQuery] string roomName)
        {
            var apt = await _userService.CreateApartmentForUserAsync(userId, roomName);
            return Ok(apt);
        }

        [HttpPost("{userId}/join-apartment")]
        public async Task<IActionResult> JoinApartment(int userId, [FromQuery] string inviteCode)
        {
            var success = await _userService.JoinApartmentByCodeAsync(userId, inviteCode);
            if (!success) return BadRequest("Érvénytelen kód!");
            return Ok();
        }

        [HttpPost("{userId}/remove-from-apartment/{apartmentId}")]
        public async Task<IActionResult> RemoveFromApartment(int userId, int apartmentId)
        {
            var success = await _userService.RemoveUserFromApartmentAsync(userId, apartmentId);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<UserDto>> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto?.IdToken))
                {
                    return BadRequest(new { message = "Hiányzó Google ID Token." });
                }

                var settings = new GoogleJsonWebSignature.ValidationSettings();
                var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, settings);

                var user = await _userService.GetOrCreateGoogleUserAsync(payload.Email, payload.Name);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Google Auth Hiba a szerveren: {ex.Message}");
                return BadRequest(new { message = $"Érvénytelen Google token: {ex.Message}" });
            }
        }
    }
}