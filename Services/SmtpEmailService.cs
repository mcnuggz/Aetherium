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

    }
}
