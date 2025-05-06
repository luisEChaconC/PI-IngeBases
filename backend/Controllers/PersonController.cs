using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonRepository _personHandler; // Repository to handle database operations for persons

        // Constructor to initialize the person repository
        public PersonController()
        {
            _personHandler = new PersonRepository(); // Initialize the person repository
        }

        /// <summary>
        /// HTTP POST endpoint to create a new person.
        /// </summary>
        /// <param name="person">The person model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreatePerson")]
        public IActionResult CreatePerson([FromBody] PersonsModel person)
        {
            try
            {
                // Validate the person model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if validation fails
                }

                // Call the repository method to create the person
                string personId = _personHandler.CreatePerson(person);

                // Return 201 Created response with a success message
                return Created("", new {id = personId, message = "Person created successfully" });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the person." });
            }
        }

        /// <summary>
        /// Creates a person dependency and returns the generated ID.
        /// </summary>
        /// <param name="person">The person model to create.</param>
        /// <returns>The ID of the created person.</returns>
        [NonAction]
        public string CreatePersonDependency(PersonsModel person)
        {
            var personResult = CreatePerson(person) as ObjectResult;
            if (personResult == null || personResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create person.");
            }

            var personId = (personResult.Value as dynamic)?.id;
            if (string.IsNullOrEmpty(personId))
            {
                throw new Exception("Failed to retrieve person ID.");
            }

            return personId;
        }
        
        /// <summary>
        /// HTTP GET endpoint to retrieve a person by its ID.
        /// </summary>
        /// <param name="id">The ID of the person to retrieve.</param>
        /// <returns>The person if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet]
        [Route("GetPersonById/{id}")]
        public IActionResult GetPersonById(string id)
        {
            try
            {
                // Call the repository method to get the person by ID
                var person = _personHandler.GetPersonById(id);

                // Check if the person was found
                if (person == null)
                {
                    // Return 404 Not Found if the person was not found
                    return NotFound(new { message = "person not found." });
                }

                // Return 200 OK response with the person
                return Ok(person);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with the error message
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "An error occurred while retrieving the person.", error = ex.Message });
            }
        }
    }
}