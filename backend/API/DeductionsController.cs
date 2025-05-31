using backend.Application;
using backend.Application.DeductionCalculation;
using backend.Application.GrossPaymentCalculation;
using backend.Infraestructure;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

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
        return Ok(new { result.gross, result.deductions });
    }
}
