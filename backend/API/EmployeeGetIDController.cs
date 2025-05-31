using backend.Infraestructure;
using backend.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeGetIDController : ControllerBase
    {
        private readonly EmployeeGetIDRepository _repository;

        public EmployeeGetIDController()
        {
            _repository = new EmployeeGetIDRepository();
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest(new { message = "Employee ID is required." });
                }

                var employee = _repository.GetEmployeeById(id);

                if (employee == null)
                {
                    return NotFound(new { message = $"Employee with ID {id} not found." });
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving the employee.",
                    error = ex.Message
                });
            }
        }
    }
}
