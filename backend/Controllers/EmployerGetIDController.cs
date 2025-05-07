using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerGetIDController : ControllerBase
    {
        private readonly EmployerGetIDRepository _repository;

        public EmployerGetIDController()
        {
            _repository = new EmployerGetIDRepository();
        }

        [HttpGet]
        [Route("GetEmployerById/{id}")]
        public IActionResult GetEmployerById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new { message = "Employer ID is required." });
                }

                var employer = _repository.GetEmployerById(id);

                if (employer == null)
                {
                    return NotFound(new { message = $"Employer with ID {id} not found." });
                }

                return Ok(employer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving the employer.",
                    error = ex.Message
                });
            }
        }
    }
}
