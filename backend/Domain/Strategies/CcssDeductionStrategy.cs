namespace backend.Domain.Strategies
{
    public class CcssDeductionStrategy : IDeductionCalculationStrategy
    {
        public decimal CalculateDeduction(decimal grossSalary, Benefit? benefit = null)
        {
            return 1000;
        }
    }
}