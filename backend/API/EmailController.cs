using backend.Application.Commands;
using backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ISendEmailCommand _sendEmailCommand;

        public EmailController(ISendEmailCommand sendEmailCommand)
        {
            _sendEmailCommand = sendEmailCommand;
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] SendEmailRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _sendEmailCommand.Execute(
                    request.To,
                    request.Subject,
                    request.Body,
                    request.AttachmentBase64,
                    request.AttachmentFilename
                );

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the email.", error = ex.Message });
            }
        }
    }

    public class SendEmailRequest
    {
        [Required(ErrorMessage = "Email recipient is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email subject is required")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email body is required")]
        public string Body { get; set; } = string.Empty;

        public string? AttachmentBase64 { get; set; }
        public string? AttachmentFilename { get; set; }
    }
}