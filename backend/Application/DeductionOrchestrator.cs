using backend.Application.DeductionCalculation;
using backend.Infraestructure;

namespace backend.Application.Orchestrators.Deduction
{
    public interface IDeductionOrchestrator
    {
        (object gross, object deductions) CalculateDeductions(CalculateDeductionDto dto);
    }

    public class DeductionOrchestrator : IDeductionOrchestrator
    {
        private readonly DeductionCalculationOrchestrator _deductionOrchestrator;
        private readonly IInsertDeductionDetailsCommand _insertDeductionDetailsCommand;
        private readonly IBenefitRepository _benefitRepository;

        public DeductionOrchestrator(
            IBenefitRepository benefitRepository,
            DeductionCalculationOrchestrator deductionOrchestrator,
            IInsertDeductionDetailsCommand insertDeductionDetailsCommand)
        {
            _benefitRepository = benefitRepository;
            _deductionOrchestrator = deductionOrchestrator;
            _insertDeductionDetailsCommand = insertDeductionDetailsCommand;
        }

        public (object gross, object deductions) CalculateDeductions(CalculateDeductionDto dto)
        {
            var benefits = _benefitRepository.GetAssignedBenefitsForEmployee(dto.EmployeeId);
            var deductions = _deductionOrchestrator.CalculateTotalDeductions(dto.GrossSalary, dto.ContractType, dto.Gender, benefits, dto.PaymentDetailsId, dto.EmployeeId);
            _insertDeductionDetailsCommand.Execute(deductions);
            return (dto.GrossSalary, deductions);
        }
    }
}