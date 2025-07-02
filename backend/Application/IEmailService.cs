namespace backend.Application.Services
{
    using System.Collections.Generic;

    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body, string? attachmentBase64 = null, string? attachmentFilename = null);
    }
}