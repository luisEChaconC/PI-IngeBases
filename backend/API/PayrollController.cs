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

        public PayrollController(IGetPayrollsByCompanyIdQuery getByCompanyIdQuery)
        {
            _getByCompanyIdQuery = getByCompanyIdQuery;
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(Guid companyId)
        {
            var details = await _getByCompanyIdQuery.ExecuteAsync(companyId);
            return Ok(details);
        }
    }
}
