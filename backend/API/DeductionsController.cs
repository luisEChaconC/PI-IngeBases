using backend.Application;
using backend.Application.DeductionCalculation;
using backend.Application.GrossPaymentCalculation;
using backend.Domain;
using backend.Infraestructure;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class DeductionsController : ControllerBase
{
    private DeductionService _deductionService;

    public DeductionsController(
        DeductionService deductionService)
    {
        _deductionService = deductionService;
    }

    [HttpPost("")]
    public IActionResult CalculateDeductions([FromBody] CalculateDeductionDto dto)
    {
        var result = _deductionService.CalculateDeductions(dto);

        var deductionsList = (List<DeductionDetailModel>)result.deductions;
        decimal totalDeductions = deductionsList.Sum(d => d.AmountDeduced);

        return Ok(new{result.gross, result.deductions, totalDeductions});
    }
}
