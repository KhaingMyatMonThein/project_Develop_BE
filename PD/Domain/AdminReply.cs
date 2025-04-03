//using PD.Models;
//using PD.Repositories;
//using PD.Services;

//namespace PD.Domain
//{
//    public class AdminReply
//    { 
//        private readonly IFormDataRepository _repository;
//        private readonly EmailService _emailService;

//        public AdminReply(IFormDataRepository repository, EmailService emailService)
//        {
//            _repository = repository;
//            _emailService = emailService;
//        }

//        public async Task<IEnumerable<ContactFormSubmission>> GetAllSubmissionsAsync()
//        {
//            return await _repository.GetAllAsync();
//        }

//        public async Task AddSubmissionAsync(ContactFormSubmission submission)
//        {
//            await _repository.AddAsync(submission);

//            // Send notification email to admin.
//            var adminEmail = "admin@example.com";
//            var subject = $"New Submission from {submission.FirstName} {submission.LastName}";
//            var body = $"Details:\n\n{submission.ProjectDescription}";

//            await _emailService.SendEmailAsync(adminEmail, subject, body);
//        }
//    }
//}
