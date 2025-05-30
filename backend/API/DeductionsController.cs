using backend.Application.DeductionCalculation;
using backend.Application.GrossPaymentCalculation;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DeductionsController : ControllerBase
{
    private readonly ICalculateGrossPaymentQuery _grossPaymentQuery;
    private readonly DeductionCalculationOrchestrator _deductionOrchestrator;
    private readonly BenefitService _benefitService;

    public DeductionsController(
        ICalculateGrossPaymentQuery grossPaymentQuery,
        DeductionCalculationOrchestrator deductionOrchestrator,
        BenefitService benefitService)
    {
        _grossPaymentQuery = grossPaymentQuery;
        _deductionOrchestrator = deductionOrchestrator;
        _benefitService = benefitService;
    }

    [HttpPost("")]
    public IActionResult CalculateDeductions([FromBody] CalculateDeductionDto dto)
    {
        var gross = _grossPaymentQuery.Execute(dto.GrossPayment);
        var benefits = _benefitService.GetAssignedBenefitsForEmployee(dto.EmployeeId);
        var deductions = _deductionOrchestrator.CalculateTotalDeductions(dto.EmployeeId, gross, benefits);

        return Ok(new { gross, deductions });
    }
}
