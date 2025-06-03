using backend.Application.DeductionCalculation;
using backend.Services;

namespace backend.Application.Orchestrators.Deduction
{
    public interface IDeductionOrchestrator
    {
        (object gross, object deductions) CalculateDeductions(CalculateDeductionDto dto);
    }

    public class DeductionOrchestrator : IDeductionOrchestrator
    {
        private readonly BenefitService _benefitService;
        private readonly DeductionCalculationOrchestrator _deductionOrchestrator;
        private readonly IInsertDeductionDetailsCommand _insertDeductionDetailsCommand;

        public DeductionOrchestrator(
            BenefitService benefitService,
            DeductionCalculationOrchestrator deductionOrchestrator,
            IInsertDeductionDetailsCommand insertDeductionDetailsCommand)
        {
            _benefitService = benefitService;
            _deductionOrchestrator = deductionOrchestrator;
            _insertDeductionDetailsCommand = insertDeductionDetailsCommand;
        }

        public (object gross, object deductions) CalculateDeductions(CalculateDeductionDto dto)
        {
            var benefits = _benefitService.GetAssignedBenefitsForEmployee(dto.EmployeeId);
            var deductions = _deductionOrchestrator.CalculateTotalDeductions(dto.GrossSalary, dto.ContractType, dto.Gender, benefits, dto.PaymentDetailsId);
            _insertDeductionDetailsCommand.Execute(deductions);
            return (dto.GrossSalary, deductions);
        }
    }
}