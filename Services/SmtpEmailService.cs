using System.Net;
using System.Net.Mail;

namespace Aetherium.Services
{
    public class SmtpEmailService
    {
        private readonly IConfiguration _config;
        public SmtpEmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string email, string subject, string body)
        {
            var fromEmail = _config["Smtp:Email"];
            var emailPass = _config["Smtp:Password"];

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, emailPass),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress("aetherium.admin@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(email);
            smtpClient.Send(message);
        }

        public void SendConfirmationEmail(string toEmail, string confirmationUrl)
        {
            var body = $"<div><p>Please confirm your account by clicking this link: <a href='{confirmationUrl}'>Confirm Email</a></p></div>";
            SendEmail(toEmail, "Confirm Your Account", body);
        }

        public void SendPasswordResetEmail(string toEmail, string resetUrl)
        {
            var body = $"<p>Click the link to reset your password: <a href='{resetUrl}'>Reset Password</a></p><p>This link will expire in <strong>12 hours</strong> for your security.</p>";
            SendEmail(toEmail, "Reset Your Password", body);
        }
    }
}
