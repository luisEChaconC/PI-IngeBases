using backend.Application.Services;

namespace backend.Application.Commands
{
    public class SendEmailCommand : ISendEmailCommand
    {
        private readonly IEmailService _emailService;

        public SendEmailCommand(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void Execute(string to, string subject, string body, string? attachmentBase64 = null, string? attachmentFilename = null, string? attachmentMimeType = null)
        {
            _emailService.SendEmail(to, subject, body, attachmentBase64, attachmentFilename, attachmentMimeType);
        }
    }
}