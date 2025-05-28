using backend.Domain.Strategies;

namespace backend.Domain.Strategies
{
    public class BiweeklyPaymentStrategy : IPaymentCalculationStrategy
    {
        public decimal CalculateGrossPayment(DateTime startDate, DateTime endDate, decimal baseSalary, int? hoursWorked = null)
        {
            int days = (endDate - startDate).Days + 1;
            return Math.Round(((baseSalary / 2) / 15m) * days, 3);
        }
    }
}
