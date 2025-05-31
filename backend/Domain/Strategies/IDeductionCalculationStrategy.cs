namespace backend.Domain.Strategies
{
    public interface IDeductionCalculationStrategy
    {
        decimal CalculateDeduction(decimal grossSalary, Benefit? benefit = null);
    }
}