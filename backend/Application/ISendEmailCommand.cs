namespace backend.Application.Commands
{
    public interface ISendEmailCommand
    {
        void Execute(string to, string subject, string body, string? attachmentBase64 = null, string? attachmentFilename = null, string? attachmentMimeType = null);
    }
}