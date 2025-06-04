namespace backend.Domain.Strategies
{
    public class CcssDeductionStrategy : IDeductionCalculationStrategy
    {
        private const decimal WorkerSemRate = 0.0550m;
        private const decimal WorkerIvmRate = 0.0417m;
        private const decimal EmployerSemRate = 0.0925m;
        private const decimal EmployerIvmRate = 0.0542m;

        private const decimal MinBaseSem = 341227m;
        private const decimal MinBaseIvm = 319384m;
       public decimal CalculateDeduction(decimal grossSalary, string contractType, string gender, Benefit? benefit = null, Guid? employeeId = null)
{
    // Exclude "Professional Services" employees from deductions
    if (contractType == "Professional Services")
        return 0;

    // Apply minimum salary base
    var semBase = Math.Max(grossSalary, MinBaseSem);
    var ivmBase = Math.Max(grossSalary, MinBaseIvm);

    // Worker deduction
    decimal semWorker = Math.Round(semBase * WorkerSemRate, 2);
    decimal ivmWorker = Math.Round(ivmBase * WorkerIvmRate, 2);
    decimal totalWorker = semWorker + ivmWorker;

    // Employer deduction (calculated, but unused for now)
    decimal semEmployer = Math.Round(semBase * EmployerSemRate, 2);
    decimal ivmEmployer = Math.Round(ivmBase * EmployerIvmRate, 2);
    decimal totalEmployer = semEmployer + ivmEmployer;

    // TODO: Store totalEmployer if needed later

    // Ensure result is non-negative
    return Math.Max(totalWorker, 0);
        }

    }
}
