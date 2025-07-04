using backend.Domain.Enums;
using backend.Application.DTOs;
using backend.Application.GrossPaymentCalculation;

namespace backend.Application.GrossPaymentCalculation
{
    public interface ICalculateGrossPaymentQuery
    {
        decimal Execute(CalculateGrossPaymentDto dto);
    }

    public class CalculateGrossPaymentQuery : ICalculateGrossPaymentQuery
    {
        private readonly GrossPaymentCalculationOrchestrator _orchestrator;

        public CalculateGrossPaymentQuery(GrossPaymentCalculationOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public decimal Execute(CalculateGrossPaymentDto dto)
        {
            return _orchestrator.CalculateGrossPayment(
                dto.EmployeeTypePayment,
                dto.BaseSalary,
                dto.StartDate,
                dto.EndDate,
                dto.WorkedHours
            );
        }
    }
}