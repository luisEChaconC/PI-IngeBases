namespace backend.Domain.Strategies
{
    public class CcssDeductionStrategy : IDeductionCalculationStrategy
    {
        public decimal CalculateDeduction(Guid employee, decimal grossSalary, Benefit? benefit = null)
        {
            return 1000;
        }
    }
}