using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PD.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUser = _configuration["EmailSettings:FromEmail"];
            var smtpPassword = _configuration["EmailSettings:Password"];

            var mailMessage = new MailMessage(smtpUser, toEmail, subject, body);
            var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
