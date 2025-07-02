using backend.Application.Queries.EmployerPayrollReport;
using backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerPayrollReportController : ControllerBase
    {
        private readonly IGetEmployerEmployeePayrollReportQuery _getEmployerEmployeePayrollReportQuery;

        public EmployerPayrollReportController(IGetEmployerEmployeePayrollReportQuery getEmployerEmployeePayrollReportQuery)
        {
            _getEmployerEmployeePayrollReportQuery = getEmployerEmployeePayrollReportQuery;
        }

        [HttpGet("employee-payroll-report")]
        public async Task<ActionResult<EmployerEmployeePayrollReportDto>> GetEmployeePayrollReport(
            Guid employerId,
            Guid companyId)
        {
            try
            {
                var result = await _getEmployerEmployeePayrollReportQuery.ExecuteAsync(companyId, employerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving employee payroll report: {ex.Message}");            
            }
        }
    }
}