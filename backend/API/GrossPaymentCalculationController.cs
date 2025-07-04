using backend.Application.GrossPaymentCalculation;
using backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/payment")]
    public class GrossPaymentCalculationController : ControllerBase
    {
        private readonly ICalculateGrossPaymentQuery _calculateGrossPaymentQuery;

        public GrossPaymentCalculationController(ICalculateGrossPaymentQuery calculateGrossPaymentQuery)
        {
            _calculateGrossPaymentQuery = calculateGrossPaymentQuery;
        }

        [HttpPost("calculate")]
        public IActionResult CalculateGrossPayment([FromBody] CalculateGrossPaymentDto dto)
        {
            var grossPayment = _calculateGrossPaymentQuery.Execute(dto);
            return Ok(new { grossPayment });
        }
    }
}