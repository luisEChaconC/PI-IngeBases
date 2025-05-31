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
        public decimal CalculateDeduction(decimal grossSalary, Benefit? benefit = null)
        {
             // Aplicar base mínima
            var semBase = Math.Max(grossSalary, MinBaseSem);
            var ivmBase = Math.Max(grossSalary, MinBaseIvm);

            // Cálculo trabajador
            decimal semWorker = Math.Round(semBase * WorkerSemRate, 2);
            decimal ivmWorker = Math.Round(ivmBase * WorkerIvmRate, 2);

            // Cálculo patrono (también aplica sobre bases mínimas)
            decimal semEmployer = Math.Round(semBase * EmployerSemRate, 2);
            decimal ivmEmployer = Math.Round(ivmBase * EmployerIvmRate, 2);

            decimal totalWorker = semWorker + ivmWorker;
            decimal totalEmployer = semEmployer + ivmEmployer;

            // TODO: almacenar employerDeduction (totalEmployer) en DeductionDetails o donde corresponda

            return totalWorker; 
        }
    }
}
