using backend.Domain.Enums;
using backend.Application.DTOs;
using backend.Application.GrossPaymentCalculation;

namespace backend.Application.Commands.SalaryCalculation
{
    public interface ICalculateSalaryCommand
    {
        decimal Execute(CalculateSalaryDto dto);
    }

    public class CalculateSalaryCommand : ICalculateSalaryCommand
    {
        private readonly GrossPaymentCalculationOrchestrator _orchestrator;

        public CalculateSalaryCommand(GrossPaymentCalculationOrchestrator orchestrator)
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