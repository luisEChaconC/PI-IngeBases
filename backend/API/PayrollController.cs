using backend.Application.Queries.Payroll;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly IGetPayrollsByCompanyIdQuery _getByCompanyIdQuery;
        private readonly IGetPayrollsSummaryByCompanyIdQuery _getSummaryByCompanyIdQuery;

        public PayrollController(IGetPayrollsByCompanyIdQuery getByCompanyIdQuery, IGetPayrollsSummaryByCompanyIdQuery getSummaryByCompanyIdQuery)
        {
            _getByCompanyIdQuery = getByCompanyIdQuery;
            _getSummaryByCompanyIdQuery = getSummaryByCompanyIdQuery;
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
