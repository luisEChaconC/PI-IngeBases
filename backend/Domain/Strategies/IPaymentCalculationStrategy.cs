using backend.Domain;

namespace backend.Domain.Strategies
{
    public interface IPaymentCalculationStrategy
    {
        decimal CalculateGrossSalary(DateTime startDate, DateTime endDate, decimal baseSalary, int? hoursWorked = null);
    }
}