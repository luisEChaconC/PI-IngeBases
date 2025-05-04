using backend.Models;
using backend.Models.Requests;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyRepository _companyHandler; // Repository to handle database operations for companies
        
        private readonly PersonController _personController; // Controller to handle person operations
        private readonly ContactController _contactController; // Controller to handle contact operations
        // Constructor to initialize the repositories
        public CompanyController()
        {
            _companyHandler = new CompanyRepository(); // Initialize the company repository
            _personController = new PersonController(); // Initialize the person controller
            _contactController = new ContactController(); // Initialize the contact controller
        }

        /// <summary>
        /// HTTP POST endpoint to create a new company.
        /// </summary>
        /// <param name="company">The company model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateCompany")]
        public IActionResult CreateCompany([FromBody] CompanyModel company)
        {
            try
            {
                // Validate the company model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if validation fails
                }

                // Call the repository method to create the company
                _companyHandler.CreateCompany(company);

                // Return 201 Created response with the ID and a success message
                return Created("", new { id = company.Id, message = "Company created successfully" });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the company." });
            }
        }

        /// <summary>
        /// HTTP POST endpoint to create a new company with all dependencies.
        /// </summary>
        /// <param name="person">The person model containing the data to insert.</param>
        /// <param name="contact">The contact model containing the data to insert.</param>
        /// <param name="company">The company model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateCompanyWithDependencies")]
        public IActionResult CreateCompanyWithDependencies([FromBody] CreateCompanyWithDependenciesRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Extract data from the request model
                var person = request.Person;
                var contact = request.Contact;
                var company = request.Company;

                // Create dependencies
                var personId = _personController.CreatePersonDependency(person);
                _contactController.CreateContactDependency(contact, personId);
                CreateCompanyDependency(company, personId);

                return Created("", new { message = "Company and all dependencies created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the company and its dependencies.", error = ex.Message });
            }
        }

        /// <summary>
        /// Creates an company dependency.
        /// </summary>
        /// <param name="company">The company model to create.</param>
        /// <param name="naturalPersonId">The ID of the associated person.</param>
        private void CreateCompanyDependency(CompanyModel company, string personId)
        {
            company.Id = personId;

            var employeeResult = CreateCompany(company) as ObjectResult;
            if (employeeResult == null || employeeResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create company.");
            }
        }

        /// <summary>
        /// HTTP GET endpoint to retrieve all companies.
        /// </summary>
        /// <returns>A list of all companies.</returns>
        [HttpGet]
        [Route("GetCompanies")]
        public IActionResult GetCompanies()
        {
            try
            {
                // Call the repository method to get all companies
                var companies = _companyHandler.GetCompanies();

                // Return 200 OK response with the companies
                return Ok(companies);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving companies." });
            }
        }

        /// <summary>
        /// HTTP GET endpoint to retrieve a company by its ID.
        /// </summary>
        /// <param name="id">The ID of the company to retrieve.</param>
        /// <returns>The company if found; otherwise, a 404 Not Found response.</returns>
        [HttpGet]
        [Route("GetCompanyById/{id}")]
        public IActionResult GetCompanyById(string id)
        {
            try
            {
                // Call the repository method to get the company by ID
                var company = _companyHandler.GetCompanyById(id);

                // Check if the company was found
                if (company == null)
                {
                    // Return 404 Not Found if the company was not found
                    return NotFound(new { message = "Company not found." });
                }

                // Return 200 OK response with the company
                return Ok(company);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the company." });
            }
        }
    }
}