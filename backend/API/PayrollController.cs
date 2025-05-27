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
            var payrolls = _payrollService.GetPayrollsByCompanyId(companyId);
            return Ok(payrolls);
        }
    }
}
