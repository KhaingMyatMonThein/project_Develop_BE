using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using PD.Dto;
using PD.Models;
using PD.Repositories;
using PD.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormDataController : ControllerBase
    {
        private readonly IFormDataRepository _formDataRepository;
        private readonly EmailService _emailService;

        public FormDataController(IFormDataRepository formDataRepository, EmailService emailService)
        {
            _formDataRepository = formDataRepository;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormData>>> GetFormData()
        {
            try
            {
                var formData = await _formDataRepository.GetFormDataAsync();
                return Ok(formData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving form data.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FormData>> PostFormData(FormDataDto formDataDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var formData = new FormData
                {
                    FirstName = formDataDto.FirstName,
                    LastName = formDataDto.LastName,
                    Email = formDataDto.Email,
                    Phone = formDataDto.Phone,
                    Company = formDataDto.Company,
                    Role = formDataDto.Role,
                    Subject = formDataDto.Subject,
                    ProjectBudget = formDataDto.ProjectBudget,
                    ProjectDescription = formDataDto.ProjectDescription,
                    Status = "Pending", 
                    Date = DateTime.UtcNow
                };

                await _formDataRepository.AddFormDataAsync(formData);
                await _emailService.SendEmailAsync(
                    formData.Email,
                    "Thank you for contacting us!",
                    $"Dear {formData.FirstName},\n\nThank you for reaching out regarding '{formData.Subject}'. We will get back to you shortly.\n\nBest regards,\nAI Solution"
                );

                return CreatedAtAction(nameof(GetFormData), new { id = formData.Id }, formData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving form data.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFormData(int id, [FromBody] UpdateStatusDto updateStatusDto)
        {
            try
            {
                var formData = await _formDataRepository.GetFormDataByIdAsync(id);
                if (formData == null)
                {
                    return NotFound();
                }

                formData.Status = updateStatusDto.NewStatus;
                await _formDataRepository.UpdateFormDataAsync(formData);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status.");
            }
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            try
            {
                await _emailService.SendEmailAsync(request.ToEmail, request.Subject, request.Message);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }
    }

}

