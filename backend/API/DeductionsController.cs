using backend.Application;
using backend.Application.DeductionCalculation;
using backend.Application.GrossPaymentCalculation;
using backend.Application.Orchestrators.Deduction;
using backend.Domain;
using backend.Infraestructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class DeductionsController : ControllerBase
{
    private readonly IDeductionOrchestrator _deductionOrchestrator;

    public DeductionsController(IDeductionOrchestrator deductionOrchestrator)
    {
        _deductionOrchestrator = deductionOrchestrator;
    }

    [HttpPost("")]
    public IActionResult CalculateDeductions([FromBody] CalculateDeductionDto dto)
    {
        var result = _deductionOrchestrator.CalculateDeductions(dto);

        var deductionsList = (List<DeductionDetailModel>)result.deductions;
        decimal totalDeductions = deductionsList.Sum(d => d.AmountDeduced);

        return Ok(new { result.deductions, totalDeductions });
    }
}
