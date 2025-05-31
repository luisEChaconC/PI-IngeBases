using backend.Domain;
using backend.Domain.Requests;
using backend.Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly EmployerRepository _employerHandler; // Repository to handle database operations for employers
        private readonly PersonController _personController;
        private readonly UserController _userController;
        private readonly NaturalPersonController _naturalPersonController;
        private readonly ContactController _contactController;

        // Constructor to initialize the repositories
        public EmployerController()
        {
            _employerHandler = new EmployerRepository();
            _personController = new PersonController();
            _userController = new UserController();
            _naturalPersonController = new NaturalPersonController();
            _contactController = new ContactController();
        }

        /// <summary>
        /// HTTP POST endpoint to create a new employer.
        /// </summary>
        /// <param name="employer">The employer model containing the data to insert.</param>
        /// <param name="naturalPerson">The natural person data for this employer.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateEmployer")]
        public IActionResult CreateEmployer([FromBody] EmployerModel employer)
        {
            try
            {
                // Validate the models
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if validation fails
                }

                // Call the repository method to create the employer
                _employerHandler.CreateEmployer(employer);

                // Return 201 Created response with the ID and a success message
                return Created("", new { id = employer.Id, message = "Employer created successfully" });

            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the employer." });
            }
        }

        /// <summary>
        /// HTTP POST endpoint to create a new employer with all dependencies.
        /// </summary>
        /// <param name="person">The person model containing the data to insert.</param>
        /// <param name="user">The user model containing the data to insert.</param>
        /// <param name="naturalPerson">The natural person model containing the data to insert.</param>
        /// <param name="contact">The contact model containing the data to insert.</param>
        /// <param name="employer">The employer model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateEmployerWithDependencies")]
        public IActionResult CreateEmployerWithDependencies([FromBody] CreateEmployerWithDependenciesRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Extract data from the request model
                var person = request.Person;
                var user = request.User;
                var naturalPerson = request.NaturalPerson;
                var contact = request.Contact;
                var employer = request.Employer;

                // Create dependencies
                var personId = _personController.CreatePersonDependency(person);
                var userId = _userController.CreateUserDependency(user);
                _naturalPersonController.CreateNaturalPersonDependency(naturalPerson, personId, userId);
                _contactController.CreateContactDependency(contact, personId);
                CreateEmployerDependency(employer, naturalPerson.Id);

                return Created("", new { message = "Employer and all dependencies created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the employer and its dependencies.", error = ex.Message });
            }
        }

        /// <summary>
        /// Creates an employer dependency.
        /// </summary>
        /// <param name="employer">The employer model to create.</param>
        /// <param name="naturalPersonId">The ID of the associated person.</param>
        [NonAction]
        private void CreateEmployerDependency(EmployerModel employer, string naturalPersonId)
        {
            employer.Id = naturalPersonId;

            var employeeResult = CreateEmployer(employer) as ObjectResult;
            if (employeeResult == null || employeeResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create employer.");
            }
        }
    }
}