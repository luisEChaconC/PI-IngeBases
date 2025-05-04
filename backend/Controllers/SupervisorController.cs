using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorController : ControllerBase
    {
        private readonly SupervisorRepository _supervisorHandler; // Repository to handle database operations for supervisors

        // Constructor to initialize the supervisor repository
        public SupervisorController()
        {
            _supervisorHandler = new SupervisorRepository(); // Initialize the supervisor repository
        }

        /// <summary>
        /// HTTP POST endpoint to create a new supervisor.
        /// </summary>
        /// <param name="supervisor">The supervisor model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateSupervisor")]
        public IActionResult CreateSupervisor([FromBody] SupervisorModel supervisor)
        {
            try
            {
                // Validate the supervisor model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if validation fails
                }

                // Call the repository method to create the supervisor
                _supervisorHandler.CreateSupervisor(supervisor);

                // Return 201 Created response with a success message
                return Created("", new {id = supervisor.Id, message = "Supervisor created successfully" });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the supervisor." });
            }
        }

        /// <summary>
        /// Creates a supervisor dependency.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to associate with the supervisor.</param>
        [NonAction]
        public void CreateSupervisorDependency(string employeeId)
        {
            SupervisorModel supervisor = new SupervisorModel { Id = employeeId };
            var supervisorResult = CreateSupervisor(supervisor) as ObjectResult;

            if (supervisorResult == null || supervisorResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create supervisor.");
            }
        }
    }
}