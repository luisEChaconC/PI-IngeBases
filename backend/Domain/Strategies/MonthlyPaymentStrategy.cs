using backend.Domain.Strategies;

namespace backend.Domain.Strategies
{
    public class MonthlyPaymentStrategy : IPaymentCalculationStrategy
    {
        public decimal CalculateGrossPayment(DateTime startDate, DateTime endDate, decimal baseSalary, int? hoursWorked = null)
        {
            int days = (endDate - startDate).Days + 1;
            return Math.Round((baseSalary / 30m) * days, 3);
        }
    }
}