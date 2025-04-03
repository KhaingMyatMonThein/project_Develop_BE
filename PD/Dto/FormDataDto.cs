using System.ComponentModel.DataAnnotations;

namespace PD.Dto
{
    public class FormDataDto
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string Phone { get; set; }

        public string Company { get; set; }
        public string Role { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }

        public string ProjectBudget { get; set; }

        [Required(ErrorMessage = "Project description is required.")]
        public string ProjectDescription { get; set; }
    }

    public class UpdateStatusDto
    {
        public string NewStatus { get; set; }
    }

    public class SendEmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
