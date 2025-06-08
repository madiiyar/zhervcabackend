using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var token = await _service.LoginAsync(dto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest($"Invalid credentials. {ex.Message}");
            }
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] string email)
        {
            try
            {
                await _service.RequestPasswordResetAsync(email);
                return Ok("Password reset OTP sent to your email.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _service.ResetPasswordAsync(dto);
                return Ok("Password has been reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int? UserId
        {
            get
            {
                var claim = User.FindFirst("id");
                if (claim == null) return null;
                if (int.TryParse(claim.Value, out var id)) return id;
                return null;
            }
        }


        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            if (UserId == null) return Unauthorized();

            var user = await _service.GetUserByIdAsync(UserId.Value);
            if (user == null) return NotFound();

            return Ok(new
            {
                user.Fullname,
                user.Email,
                user.Phonenumber,
                user.Role
            });
        }


        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            if (UserId == null) return Unauthorized();

            try
            {
                await _service.UpdateUserProfileAsync(UserId.Value, dto);
                return Ok("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (UserId == null) return Unauthorized();

            try
            {
                await _service.ChangePasswordAsync(UserId.Value, dto.OldPassword, dto.NewPassword);
                return Ok("Password changed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
