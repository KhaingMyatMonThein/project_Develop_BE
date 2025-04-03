using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PD.Data;
using PD.Models;
using BCrypt.Net;
using System.Threading.Tasks;
using System;
using PD.Services;

namespace PD.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOtpService _otpService;

        public AuthController(ApplicationDbContext context, IOtpService otpService)
        {
            _context = context;
            _otpService = otpService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
     
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            try
            {

                await _otpService.SendOtpAsync(request.Email);
                return Ok(new { message = "OTP sent successfully" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Failed to send OTP. Please try again." });
            }
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] OtpRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isValid = _otpService.ValidateOtp(request.Email, request.Otp);
                if (!isValid)
                {
                    return BadRequest(new { message = "Invalid or expired OTP" });
                }

                return Ok(new { message = "Login successful" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in VerifyOtp: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while verifying OTP" });
            }
        }


    }
}
