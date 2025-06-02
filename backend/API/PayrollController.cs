using backend.Application.Orchestrators.Payroll;
using backend.Application.Queries.Payroll;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollOrchestrator _payrollOrchestrator;
        private readonly IGetPayrollsByCompanyIdQuery _getByCompanyIdQuery;
        private readonly IGetPayrollsSummaryByCompanyIdQuery _getSummaryByCompanyIdQuery;

        public PayrollController(IPayrollOrchestrator payrollOrchestrator, IGetPayrollsByCompanyIdQuery getByCompanyIdQuery, IGetPayrollsSummaryByCompanyIdQuery getSummaryByCompanyIdQuery)
        {
            _payrollOrchestrator = payrollOrchestrator;
            _getByCompanyIdQuery = getByCompanyIdQuery;
            _getSummaryByCompanyIdQuery = getSummaryByCompanyIdQuery;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PayrollModel model)
        {
            try
            {
                if (model.CompanyId == Guid.Empty)
                    return BadRequest("CompanyId is required.");

                if (model.PayrollManagerId == Guid.Empty)
                    return BadRequest("PayrollManagerId is required.");

                if (model.StartDate == default)
                    return BadRequest("StartDate is required.");

                if (model.EndDate == default)
                    return BadRequest("EndDate is required.");

                if (model.EndDate <= model.StartDate)
                    return BadRequest("EndDate must be greater than StartDate.");

                var id = await _payrollOrchestrator.GeneratePayroll(model);
                return Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while creating the payroll",
                    error = ex.Message
                });
            }
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(Guid companyId)
        {
            try
            {
                if (companyId == Guid.Empty)
                {
                    return BadRequest("CompanyId is required");
                }

                var details = await _getByCompanyIdQuery.ExecuteAsync(companyId);
                return Ok(details);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving the payrolls",
                    error = ex.Message
                });
            }
        }

        [HttpGet("company/{companyId}/summary")]
        public async Task<IActionResult> GetSummaryByCompanyId(Guid companyId)
        {
            try
            {
                if (companyId == Guid.Empty)
                {
                    return BadRequest("CompanyId is required");
                }
                var details = await _getSummaryByCompanyIdQuery.ExecuteAsync(companyId);
                return Ok(details);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving the payrolls summary",
                    error = ex.Message
                });
            }
        }
    }
}
