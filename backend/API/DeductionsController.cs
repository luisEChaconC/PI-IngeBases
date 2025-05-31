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
    private readonly ICalculateGrossPaymentQuery _grossPaymentQuery;
    private readonly DeductionCalculationOrchestrator _deductionOrchestrator;
    private readonly BenefitService _benefitService;
    private readonly IInsertDeductionDetailsCommand _insertDeductionDetailsCommand;

    public DeductionsController(
        ICalculateGrossPaymentQuery grossPaymentQuery,
        DeductionCalculationOrchestrator deductionOrchestrator,
        BenefitService benefitService, IInsertDeductionDetailsCommand insertDeductionDetailsCommand)
    {
        _grossPaymentQuery = grossPaymentQuery;
        _deductionOrchestrator = deductionOrchestrator;
        _benefitService = benefitService;
        _insertDeductionDetailsCommand = insertDeductionDetailsCommand;
    }

    [HttpPost("")]
    public IActionResult CalculateDeductions([FromBody] CalculateDeductionDto dto)
    {
        var gross = _grossPaymentQuery.Execute(dto.GrossPayment);
        var deductions = _deductionOrchestrator.CalculateTotalDeductions(gross, dto.Benefits, dto.PaymentDetailsId);
        
        _insertDeductionDetailsCommand.Execute(deductions);

        return Ok(new { gross, deductions });
    }
}
