
namespace backend.Domain.Strategies
{
    public class IncomeTaxDeductionStrategy : IDeductionCalculationStrategy
    {
        public decimal CalculateDeduction(decimal grossSalary, string contractType, Benefit? benefit = null)
        {
            if (contractType == "Professional Services")
                return 0;
                
            decimal totalTax = 0;

            if (grossSalary > 4_745_000m)
            {
                totalTax += (grossSalary - 4_745_000m) * 0.25m;
                grossSalary = 4_745_000m;
            }

            if (grossSalary > 2_373_000m)
            {
                totalTax += (grossSalary - 2_373_000m) * 0.20m;
                grossSalary = 2_373_000m;
            }

            if (grossSalary > 1_352_000m)
            {
                totalTax += (grossSalary - 1_352_000m) * 0.15m;
                grossSalary = 1_352_000m;
            }

            if (grossSalary > 922_000m)
            {
                totalTax += (grossSalary - 922_000m) * 0.10m;
                
            }

            return Math.Round(totalTax, 2);
        }
    }
}
