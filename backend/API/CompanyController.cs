using backend.Domain;
using backend.Domain.Requests;
using backend.Infraestructure;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyRepository _companyRepository; // Repository to handle database operations for companies

        private readonly PersonController _personController; // Controller to handle person operations
        private readonly ContactController _contactController; // Controller to handle contact operations
        // Constructor to initialize the repositories
        public CompanyController()
        {
            var personRepo = new PersonRepository();
            var contactRepo = new ContactRepository();
            var employeeRepo = new EmployeeRepository(); // Initialize the employee repository
            _companyRepository = new CompanyRepository(personRepo, contactRepo, employeeRepo); // Initialize the company repository

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
                _companyRepository.CreateCompany(company);

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
        /// <param name="request">The request model containing the person, contacts, and company data.</param>
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
                var contacts = request.Contacts;
                var company = request.Company;

                // Create dependencies
                // Create person dependency and get the generated ID
                var personId = _personController.CreatePersonDependency(person);

                // Create contact associated with the person
                foreach (var contact in contacts)
                {
                    _contactController.CreateContactDependency(contact, personId);
                }

                // Create the company
                CreateCompanyDependency(company, personId);

                return Created("", new { message = "Company and all dependencies created successfully", personId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the company and its dependencies.", error = ex.Message });
            }
        }

        /// <summary>
        /// Creates a company dependency.
        /// </summary>
        /// <param name="company">The company model to create.</param>
        /// <param name="personId">The ID of the associated person.</param>
        private void CreateCompanyDependency(CompanyModel company, string personId)
        {
            company.Id = personId;

            var companyResult = CreateCompany(company) as ObjectResult;
            if (companyResult == null || companyResult.StatusCode != StatusCodes.Status201Created)
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
                var companies = _companyRepository.GetCompanies();

                // Return 200 OK response with the companies
                return Ok(companies);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with the error message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving companies.", error = ex.Message });
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
                var company = _companyRepository.GetCompanyById(id);

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
                // Return a 500 Internal Server Error response with the error message
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving the company.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCompany(string id, [FromBody] UpdateCompanyModel company)
        {
            try
            {
                if (id != company.Id)
                    return BadRequest("El ID de la compañía no coincide con el ID proporcionado en el URL");

                _companyRepository.UpdateCompany(company);
                return Ok("La compañía ha sido actualizada correctamente");
            }
            catch (Exception ex)
            {
                // Check for known validation errors
                if (ex.Message.Contains("ya existe"))
                    return Conflict(new { message = ex.Message }); 

                return StatusCode(500, $"An update error occurred: {ex.Message}");
            }
        }

    }
}