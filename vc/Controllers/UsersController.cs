using Microsoft.AspNetCore.Mvc;
using vc.DTOs;
using vc.Services;

namespace vc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            try
            {
                await _service.RegisterAsync(dto);
                return Ok("OTP sent to your email.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email already registered"))
                    return BadRequest("This email is already in use.");

                return StatusCode(500, "An error occurred during registration.");
            }
        }


        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(OtpVerifyDto dto)
        {
            await _service.VerifyOtpAsync(dto);
            return Ok("Email confirmed.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _service.LoginAsync(dto);
            return Ok(new { Token = token });
        }
    }
}
