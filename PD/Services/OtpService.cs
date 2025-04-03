using System;
using System.Net.Mail;
using System.Net;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PD.Services
{
    public class OtpService : IOtpService
    {
        private static readonly ConcurrentDictionary<string, (string Otp, DateTime Expiry)> _otpStore = new();
        private readonly SmtpClient _smtpClient;

        public OtpService(string smtpHost, int smtpPort, string smtpUsername, string smtpPassword)
        {
            _smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };
        }

        public async Task SendOtpAsync(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.AddMinutes(5); // OTP valid for 5 minutes
            _otpStore[email] = (otp, expiry);

            var message = new MailMessage("monix.pec@gmail.com", email)
            {
                Subject = "Your Login OTP",
                Body = $"Your OTP is: {otp} (Valid for 5 minutes)"
            };

            await _smtpClient.SendMailAsync(message);
        }

        public bool ValidateOtp(string email, string otp)
        {
            if (_otpStore.TryGetValue(email, out var storedOtp))
            {
                if (storedOtp.Otp == otp && DateTime.UtcNow <= storedOtp.Expiry)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
