namespace vc.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

}
