using backend.Domain;
using backend.Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollManagerController : ControllerBase
    {
        private readonly PayrollManagerRepository _payrollManagerRepository; // Repository to handle database operations for payroll managers

        // Constructor to initialize the payroll manager repository
        public PayrollManagerController()
        {
            _payrollManagerRepository = new PayrollManagerRepository(); // Initialize the payroll manager repository
        }

        /// <summary>
        /// HTTP POST endpoint to create a new payroll manager.
        /// </summary>
        /// <param name="payrollManager">The payroll manager model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreatePayrollManager")]
        public IActionResult CreatePayrollManager([FromBody] PayrollManagerModel payrollManager)
        {
            try
            {
                // Validate the payroll manager model
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if validation fails
                }

                // Call the repository method to create the payroll manager
                _payrollManagerRepository.CreatePayrollManager(payrollManager);

                // Return 201 Created response with a success message
                return Created("", new { id = payrollManager.Id, message = "Payroll Manager created successfully" });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with a generic error message
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the payroll manager." });
            }
        }

        /// <summary>
        /// Creates a payroll manager dependency.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to associate with the payroll manager.</param>
        [NonAction]
        public void CreatePayrollManagerDependency(string employeeId)
        {
            PayrollManagerModel payrollManager = new PayrollManagerModel { Id = employeeId };
            var payrollManagerResult = CreatePayrollManager(payrollManager) as ObjectResult;

            if (payrollManagerResult == null || payrollManagerResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create payroll manager.");
            }
        }
    }
}