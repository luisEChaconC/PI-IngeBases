namespace backend.Domain.Strategies
{
    public class CcssDeductionStrategy : IDeductionCalculationStrategy
    {
        public decimal CalculateDeduction(decimal grossSalary, Benefit? benefit = null)
        {
            return Math.Round(grossSalary * 0.09m, 2);
        }
    }
}