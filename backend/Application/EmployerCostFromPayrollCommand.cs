using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Commands.Payroll
{
    public class EmployerCostFromPayrollCommand : IEmployerCostFromPayrollCommand
    {
        private readonly IGetGrossSalaryByPayrollIdQuery _grossSalaryQuery;
        private readonly IEmployerCostRepository _employerCostRepository;
        private readonly IEmployerCostStrategy _strategy;

        public EmployerCostFromPayrollCommand(
            IGetGrossSalaryByPayrollIdQuery grossSalaryQuery,
            IEmployerCostRepository employerCostRepository,
            IEmployerCostStrategy strategy)
        {
            _grossSalaryQuery = grossSalaryQuery;
            _employerCostRepository = employerCostRepository;
            _strategy = strategy;
        }



        public void Execute(Guid payrollId)
        {
            var grossSalary = _grossSalaryQuery.Execute(payrollId);
            var costModel = _strategy.Calculate(payrollId, grossSalary);
            costModel.TotalEmployerCost = costModel.LegalDeductionsTotal + grossSalary;
            costModel.PrivateInsurance = grossSalary;
            _employerCostRepository.Insert(costModel);
        }
    }
}
