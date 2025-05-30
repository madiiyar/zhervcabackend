using MailKit.Net.Smtp;
using MimeKit;

namespace vc.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_config["Smtp:FromName"], _config["Smtp:Username"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Your ZHER VC OTP Code";

            message.Body = new TextPart("plain")
            {
                Text = $"Your verification code is: {otp}\nThis code is valid for 10 minutes."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]), false);
            await client.AuthenticateAsync(_config["Smtp:Username"], _config["Smtp:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
