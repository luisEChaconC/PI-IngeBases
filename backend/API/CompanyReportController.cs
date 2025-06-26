using backend.Domain;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyReportController : ControllerBase
    {
        private readonly IGetCompanyReportsQuery _getCompanyReportsQuery;
        public CompanyReportController(IGetCompanyReportsQuery getCompanyReportsQuery)
        {
            _getCompanyReportsQuery = getCompanyReportsQuery;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var reports = await _getCompanyReportsQuery.ExecuteAsync(startDate, endDate);
            return Ok(reports);
        }
    }
}