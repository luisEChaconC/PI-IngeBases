namespace backend.Domain.Strategies
{
    public interface IDeductionCalculationStrategy
    {
        decimal CalculateDeduction(decimal grossSalary, string contractType, Benefit? benefit = null);
    }
}