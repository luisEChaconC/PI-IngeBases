using backend.Domain;
using backend.Domain.Requests;
using backend.Application.Commands.Employee;
using backend.Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly SupervisorController _supervisorController;
        private readonly PayrollManagerController _payrollManagerController;
        private readonly PersonController _personController;
        private readonly UserController _userController;
        private readonly NaturalPersonController _naturalPersonController;
        private readonly ContactController _contactController;
        private readonly IDeleteEmployeeCommand _deleteEmployeeCommand;


        public EmployeeController()
        {
            _employeeRepository = new EmployeeRepository();
            _deleteEmployeeCommand = new DeleteEmployeeCommand(_employeeRepository);
            _supervisorController = new SupervisorController();
            _payrollManagerController = new PayrollManagerController();
            _personController = new PersonController();
            _userController = new UserController();
            _naturalPersonController = new NaturalPersonController();
            _contactController = new ContactController();
        }

        /// <summary>
        /// HTTP POST endpoint to create a new employee.
        /// </summary>
        /// <param name="employee">The employee model containing the data to insert.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateEmployee")]
        public IActionResult CreateEmployee([FromBody] EmployeeModel employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _employeeRepository.CreateEmployee(employee);

                return Created("", new { id = employee.Id, message = "Employee created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the employee." });
            }
        }

        /// <summary>
        /// HTTP POST endpoint to create a new employee with all dependencies.
        /// </summary>
        /// <param name="person">The person model containing the data to insert.</param>
        /// <param name="user">The user model containing the data to insert.</param>
        /// <param name="naturalPerson">The natural person model containing the data to insert.</param>
        /// <param name="contact">The contact model containing the data to insert.</param>
        /// <param name="employee">The employee model containing the data to insert.</param>
        /// <param name="employeeRole">The role of the employee.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("CreateEmployeeWithDependencies")]
        public IActionResult CreateEmployeeWithDependencies([FromBody] CreateEmployeeWithDependenciesRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(request.EmployeeRole))
                {
                    return BadRequest(new { message = "Employee role is required." });
                }

                // Extract data from the request model
                var person = request.Person;
                var user = request.User;
                var naturalPerson = request.NaturalPerson;
                var contact = request.Contact;
                var employee = request.Employee;

                // Create dependencies
                var personId = _personController.CreatePersonDependency(person);
                var userId = _userController.CreateUserDependency(user);
                _naturalPersonController.CreateNaturalPersonDependency(naturalPerson, personId, userId);
                _contactController.CreateContactDependency(contact, personId);
                CreateEmployeeDependency(employee, naturalPerson.Id);

                // Handle role-specific creation
                if (request.EmployeeRole == "Supervisor")
                {
                    _supervisorController.CreateSupervisorDependency(employee.Id.ToString());
                }
                else if (request.EmployeeRole == "Payroll Manager")
                {
                    _payrollManagerController.CreatePayrollManagerDependency(employee.Id.ToString());
                }

                return Created("", new { message = "Employee and all dependencies created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the employee and its dependencies.", error = ex.Message });
            }
        }

        /// <summary>
        /// Creates an employee dependency.
        /// </summary>
        /// <param name="employee">The employee model to create.</param>
        /// <param name="naturalPersonId">The ID of the associated person.</param>
        [NonAction]
        private void CreateEmployeeDependency(EmployeeModel employee, string naturalPersonId)
        {
            employee.Id = Guid.Parse(naturalPersonId);

            var employeeResult = CreateEmployee(employee) as ObjectResult;
            if (employeeResult == null || employeeResult.StatusCode != StatusCodes.Status201Created)
            {
                throw new Exception("Failed to create employee.");
            }
        }

        /// <summary>
        /// HTTP GET endpoint to retrieve employees by company ID.
        /// </summary>
        /// <param name="companyId">The ID of the company.</param>
        /// <returns>A list of employees associated with the specified company.</returns>
        [HttpGet]
        [Route("GetEmployeesByCompanyId")]
        public IActionResult GetEmployeesByCompanyId(string companyId)
        {
            try
            {
                if (string.IsNullOrEmpty(companyId))
                {
                    return BadRequest(new { message = "Company ID is required." });
                }

                // Retrieve employees from the repository
                var employees = _employeeRepository.GetEmployeesByCompanyId(companyId);

                if (employees == null || !employees.Any())
                {
                    return NotFound(new { message = "No employees found for the specified company ID." });
                }

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving employees.", error = ex.Message });
            }
        }

[HttpDelete("{id}")]
public IActionResult DeleteEmployee(string id)
{
    try
    {
        _deleteEmployeeCommand.Execute(id);
        return Ok(new { message = "Empleado eliminado correctamente." });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "Error al eliminar el empleado.", error = ex.Message });
    }
}



    }
}