using backend.Application.Services;
using backend.Application.DTOs;

namespace backend.Application.Commands
{
    public class SendEmailCommand : ISendEmailCommand
    {
        private readonly IEmailService _emailService;

        public SendEmailCommand(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void Execute(string to, string subject, string body, string? attachmentBase64 = null, string? attachmentFilename = null)
        {
            if (!string.IsNullOrWhiteSpace(attachmentBase64))
            {
                if (attachmentFilename == null)
                    throw new ArgumentNullException(nameof(attachmentFilename), "Attachment filename is required when attachment data is provided");

                if (string.IsNullOrWhiteSpace(attachmentFilename))
                    throw new ArgumentException("Attachment filename cannot be empty", nameof(attachmentFilename));
            }

            _emailService.SendEmail(
                to,
                subject,
                body,
                attachmentBase64,
                attachmentFilename);
        }
    }
}