using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using backend.Application.Services;

namespace backend.Infraestructure.service
{
    public class EmailService : IEmailService
    {
        private const int SMTP_TIMEOUT_MILLISECONDS = 30000;

        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _smtpHost = GetRequiredConfiguration(configuration, "Smtp:Host");
            _smtpPort = int.Parse(GetRequiredConfiguration(configuration, "Smtp:Port"));
            _smtpUsername = GetRequiredConfiguration(configuration, "Smtp:Username");
            _smtpPassword = GetRequiredConfiguration(configuration, "Smtp:Password");
            _enableSsl = bool.Parse(GetRequiredConfiguration(configuration, "Smtp:EnableSsl"));
            _fromEmail = GetRequiredConfiguration(configuration, "Smtp:FromEmail");
            _fromName = GetRequiredConfiguration(configuration, "Smtp:FromName");
        }

        public void SendEmail(string to, string subject, string body, string? attachmentBase64 = null, string? attachmentFilename = null, string? attachmentMimeType = null)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_fromEmail, _fromName);
                    mailMessage.To.Add(to);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = false;

                    if (!string.IsNullOrEmpty(attachmentBase64) && !string.IsNullOrEmpty(attachmentFilename))
                    {
                        byte[] attachmentBytes = Convert.FromBase64String(attachmentBase64);
                        var attachmentStream = new MemoryStream(attachmentBytes);
                        var attachment = new Attachment(attachmentStream,
                                                        attachmentFilename,
                                                        string.IsNullOrWhiteSpace(attachmentMimeType) ? "application/octet-stream" : attachmentMimeType);
                        mailMessage.Attachments.Add(attachment);
                    }

                    DeliverEmailMessage(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send email to {to}: {ex.Message}", ex);
            }
        }

        public void SendEmailBatch(List<string> to, string subject, string body)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_fromEmail, _fromName);

                    foreach (var recipient in to)
                    {
                        if (!string.IsNullOrWhiteSpace(recipient))
                        {
                            mailMessage.To.Add(recipient);
                        }
                    }

                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = false;

                    DeliverEmailMessage(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send batch email to {to.Count} recipients: {ex.Message}", ex);
            }
        }

        private string GetRequiredConfiguration(IConfiguration configuration, string key)
        {
            var value = configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Required configuration '{key}' is not defined. Please ensure this setting is configured in user secrets or environment variables.");
            }
            return value;
        }

        private void DeliverEmailMessage(MailMessage mailMessage)
        {
            using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                smtpClient.EnableSsl = _enableSsl;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Timeout = SMTP_TIMEOUT_MILLISECONDS;

                smtpClient.Send(mailMessage);
            }
        }
    }
}
