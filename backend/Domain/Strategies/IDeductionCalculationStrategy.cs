namespace backend.Domain.Strategies
{
    public interface IDeductionCalculationStrategy
    {
        decimal CalculateDeduction(Guid employee, decimal grossSalary, Benefit? benefit = null);
    }
}