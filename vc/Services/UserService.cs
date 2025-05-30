using vc.Auth;
using vc.DTOs;
using vc.Models;
using Microsoft.EntityFrameworkCore;
using vc.Services;

namespace vc.Services
{
    public class UserService
    {
        private readonly VcdbContext _context;
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly EmailService _email;

        public UserService(VcdbContext context, JwtTokenGenerator tokenGenerator, EmailService email)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _email = email; 
        }

        public async Task RegisterAsync(RegisterUserDto dto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (exists) throw new Exception("Email already registered.");

            var user = new User
            {
                Fullname = dto.FullName,
                Email = dto.Email,
                Phonenumber = dto.PhoneNumber,
                Passwordhash = PasswordHasher.Hash(dto.Password),
                Role = dto.Role,
                Isemailconfirmed = false,
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var otp = new Emailotp
            {
                Userid = user.Id,
                Otpcode = GenerateOtp(),
                Purpose = "Register",
                Expiresat = DateTime.UtcNow.AddMinutes(10),
                Createdat = DateTime.UtcNow
            };

            _context.Emailotps.Add(otp);
            await _context.SaveChangesAsync();

            //  Send OTP via email
            await _email.SendOtpEmailAsync(user.Email, otp.Otpcode);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) throw new Exception("User not found.");

            if (user.Isemailconfirmed != true)
                throw new Exception("Email is not confirmed. Please verify OTP.");

            var valid = PasswordHasher.Verify(dto.Password, user.Passwordhash);
            if (!valid) throw new Exception("Invalid password.");

            return _tokenGenerator.GenerateToken(user.Id, user.Role);
        }

        public async Task VerifyOtpAsync(OtpVerifyDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) throw new Exception("User not found.");

            var otp = await _context.Emailotps
                .Where(o => o.Userid == user.Id &&
                            o.Purpose == "Register" &&
                            o.Isused == false &&
                            o.Expiresat > DateTime.UtcNow)
                .OrderByDescending(o => o.Createdat)
                .FirstOrDefaultAsync();

            if (otp == null || otp.Otpcode != dto.OtpCode)
                throw new Exception("Invalid or expired OTP.");

            otp.Isused = true;
            user.Isemailconfirmed = true;
            user.Updatedat = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        private string GenerateOtp()
        {
            var rand = new Random();
            return rand.Next(100000, 999999).ToString(); // 6-digit OTP
        }
    }
}
