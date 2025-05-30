namespace backend.Domain.Strategies
{
    public class IncomeTaxDeductionStrategy : IDeductionCalculationStrategy
    {
        public decimal CalculateDeduction(Guid employee, decimal grossSalary, Benefit? benefit = null)
        {
            return 2000;
        }
    }
}