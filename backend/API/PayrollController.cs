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
            var details = await _getByCompanyIdQuery.ExecuteAsync(companyId);
            return Ok(details);
        }

        [HttpGet("company/{companyId}/summary")]
        public async Task<IActionResult> GetSummaryByCompanyId(Guid companyId)
        {
            var details = await _getSummaryByCompanyIdQuery.ExecuteAsync(companyId);
            return Ok(details);
        }
    }
}
