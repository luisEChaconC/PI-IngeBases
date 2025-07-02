using NUnit.Framework;
using Moq;
using backend.Application.Commands;
using backend.Application.Services;
using System;

namespace TestPayrollSystem
{
    [TestFixture]
    public class SendEmailCommandTest
    {
        private Mock<IEmailService> _mockEmailService;
        private SendEmailCommand _command;

        [SetUp]
        public void SetUp()
        {
            _mockEmailService = new Mock<IEmailService>();
            _command = new SendEmailCommand(_mockEmailService.Object);
        }

        [Test]
        public void Execute_CallsEmailServiceSendEmail_WithCorrectParameters()
        {
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            _command.Execute(to, subject, body);

            _mockEmailService.Verify(s => s.SendEmail(to, subject, body, null, null), Times.Once);
        }

        [Test]
        public void Execute_CallsEmailServiceSendEmail_WithCorrectParametersWithAttachment()
        {
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";
            var attachmentBase64 = "dGVzdCBkYXRh";
            var attachmentFilename = "test.txt";

            _command.Execute(to, subject, body, attachmentBase64, attachmentFilename);

            _mockEmailService.Verify(s => s.SendEmail(to, subject, body, attachmentBase64, attachmentFilename), Times.Once);
        }

        [Test]
        public void Execute_ThrowsArgumentNullException_WhenAttachmentBase64ProvidedButFilenameIsNull()
        {
            var attachmentBase64 = "dGVzdCBkYXRh";

            Assert.Throws<ArgumentNullException>(() => _command.Execute("test@example.com", "Subject", "Body", attachmentBase64, null));
        }

        [Test]
        public void Execute_ThrowsArgumentException_WhenAttachmentBase64ProvidedButFilenameIsEmpty()
        {
            var attachmentBase64 = "dGVzdCBkYXRh";

            Assert.Throws<ArgumentException>(() => _command.Execute("test@example.com", "Subject", "Body", attachmentBase64, string.Empty));
        }

        [Test]
        public void Execute_DoesNotCallEmailService_WhenAttachmentValidationFails()
        {
            var mockEmailService = new Mock<IEmailService>();
            var command = new SendEmailCommand(mockEmailService.Object);
            var attachmentBase64 = "dGVzdCBkYXRh";

            Assert.Throws<ArgumentNullException>(() => command.Execute("test@example.com", "Subject", "Body", attachmentBase64, null));
            mockEmailService.Verify(s => s.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Execute_PropagatesException_FromEmailService()
        {
            var mockEmailService = new Mock<IEmailService>();
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";
            mockEmailService.Setup(s => s.SendEmail(to, subject, body, null, null)).Throws(new InvalidOperationException("Email service error"));

            var command = new SendEmailCommand(mockEmailService.Object);

            Assert.Throws<InvalidOperationException>(() => command.Execute(to, subject, body));
        }
    }
}