using backend.Application;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpGet]
        public IActionResult GetPayrollsByCompanyId([FromQuery] string companyId)
        {
            try
            {
                if (string.IsNullOrEmpty(companyId))
                {
                    return BadRequest("CompanyId is required.");
                }

                var payrolls = _payrollService.GetPayrollsByCompanyId(companyId);
                return Ok(payrolls);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving the payrolls.",
                    error = ex.Message
                });
            }
        }
    }
}
