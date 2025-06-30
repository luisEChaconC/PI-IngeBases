using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Commands.Payroll
{
    public class EmployerCostFromPayrollCommand : IEmployerCostFromPayrollCommand
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployerCostRepository _employerCostRepository;
        private readonly IEmployerCostStrategy _strategy;

        public EmployerCostFromPayrollCommand(
            IPayrollRepository payrollRepository,
            IEmployerCostRepository employerCostRepository,
            IEmployerCostStrategy strategy)
        {
            _payrollRepository = payrollRepository;
            _employerCostRepository = employerCostRepository;
            _strategy = strategy;
        }

        public void Execute(Guid payrollId)
        {
            var grossSalary = _payrollRepository.GetTotalGrossSalaryByPayrollId(payrollId);
            var costModel = _strategy.Calculate(payrollId, grossSalary);
            _employerCostRepository.Insert(costModel);
        }
    }
}
