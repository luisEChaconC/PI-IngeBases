using backend.Application.Commands.SalaryCalculation;
using backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/salary")]
    public class SalaryCalculationController : ControllerBase
    {
        private readonly ICalculateSalaryCommand _calculateSalaryCommand;

        public SalaryCalculationController(ICalculateSalaryCommand calculateSalaryCommand)
        {
            _calculateSalaryCommand = calculateSalaryCommand;
        }

        [HttpPost("calculate")]
        public IActionResult CalculateSalary([FromBody] CalculateSalaryDto dto)
        {
            var grossSalary = _calculateSalaryCommand.Execute(dto);
            return Ok(new { grossSalary });
        }
    }
}