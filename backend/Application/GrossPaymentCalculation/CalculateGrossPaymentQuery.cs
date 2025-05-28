using backend.Domain.Enums;
using backend.Application.DTOs;
using backend.Application.GrossPaymentCalculation;

namespace backend.Application.GrossPaymentCalculation
{
    public interface ICalculateGrossPaymentQuery
    {
        decimal Execute(CalculateSalaryDto dto);
    }

    public class CalculateGrossPaymentQuery : ICalculateGrossPaymentQuery
    {
        private readonly GrossPaymentCalculationOrchestrator _orchestrator;

        public CalculateGrossPaymentQuery(GrossPaymentCalculationOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public decimal Execute(CalculateSalaryDto dto)
        {
            return _orchestrator.CalculateGrossSalary(
                dto.EmployeeTypePayment,
                dto.BaseSalary,
                dto.StartDate,
                dto.EndDate,
                dto.WorkedHours
            );
        }
    }
}