using backend.Domain.Strategies;

namespace backend.Domain.Strategies
{
    public class WeeklyPaymentStrategy : IPaymentCalculationStrategy
    {
        public decimal CalculateGrossPayment(DateTime startDate, DateTime endDate, decimal baseSalary, int? hoursWorked = null)
        {
            if (hoursWorked == null)
                throw new ArgumentException("Hours worked must be provided for weekly employees.");

            return Math.Round(baseSalary * hoursWorked.Value, 3);
        }
    }
}