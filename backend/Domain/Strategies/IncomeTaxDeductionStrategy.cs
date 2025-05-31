namespace backend.Domain.Strategies
{
    public class IncomeTaxDeductionStrategy : IDeductionCalculationStrategy
    {
        public decimal CalculateDeduction(decimal grossSalary, Benefit? benefit = null)
        {
            return 2000;
        }
    }
}