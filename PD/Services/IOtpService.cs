using System.Threading.Tasks;

namespace PD.Services
{
    public interface IOtpService
    {
        Task SendOtpAsync(string email);
        bool ValidateOtp(string email, string otp);
    }
}
