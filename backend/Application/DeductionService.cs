using backend.Application.DeductionCalculation;
using backend.Application.GrossPaymentCalculation;
using backend.Services;

public class DeductionService
{
    private readonly ICalculateGrossPaymentQuery _grossPaymentQuery;
    private readonly BenefitService _benefitService;
    private readonly DeductionCalculationOrchestrator _deductionOrchestrator;
    private readonly IInsertDeductionDetailsCommand _insertDeductionDetailsCommand;

    public DeductionService(
        ICalculateGrossPaymentQuery grossPaymentQuery,
        BenefitService benefitService,
        DeductionCalculationOrchestrator deductionOrchestrator,
        IInsertDeductionDetailsCommand insertDeductionDetailsCommand)
    {
        _grossPaymentQuery = grossPaymentQuery;
        _benefitService = benefitService;
        _deductionOrchestrator = deductionOrchestrator;
        _insertDeductionDetailsCommand = insertDeductionDetailsCommand;
    }

    public (object gross, object deductions) CalculateDeductions(CalculateDeductionDto dto)
    {
        var gross = _grossPaymentQuery.Execute(dto.GrossPayment);
        var benefits = _benefitService.GetAssignedBenefitsForEmployee(dto.EmployeeId);
        var deductions = _deductionOrchestrator.CalculateTotalDeductions(gross, benefits, dto.PaymentDetailsId);
        _insertDeductionDetailsCommand.Execute(deductions);
        return (gross, deductions);
    }
}