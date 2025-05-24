using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NaturalPersonController : ControllerBase
    {
        private readonly NaturalPersonRepository _naturalPersonRepository; // Repository to handle database operations for natural persons

        // Constructor to initialize the repositories
        public NaturalPersonController()
        {
            _naturalPersonRepository = new NaturalPersonRepository(); // Initialize the natural person repository
        }

        /// <summary>
        /// HTTP POST endpoint to create a new natural person.
        /// </summary>
        /// <param name="naturalPerson">The natural person model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateNaturalPerson")]
        public IActionResult CreateNaturalPerson([FromBody] NaturalPersonModel naturalPerson)
        {
            try
            {
                // Validate the natural person model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if validation fails
                }

                // Call the repository method to create the natural person
                _naturalPersonRepository.CreateNaturalPerson(naturalPerson);

                // Return 201 Created response with the ID and a success message
                return Created("", new { id = naturalPerson.Id, message = "Natural person created successfully" });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the natural person." });
            }
        }

        /// <summary>
        /// Creates a natural person dependency.
        /// </summary>
        /// <param name="naturalPerson">The natural person model to create.</param>
        /// <param name="personId">The ID of the associated person.</param>
        /// <param name="userId">The ID of the associated user.</param>
        [NonAction]
        public void CreateNaturalPersonDependency(NaturalPersonModel naturalPerson, string personId, string userId)
        {
            naturalPerson.Id = personId;
            naturalPerson.UserId = userId;

            var naturalPersonResult = CreateNaturalPerson(naturalPerson) as ObjectResult;
            if (naturalPersonResult == null || naturalPersonResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create natural person.");
            }
        }
    }
}