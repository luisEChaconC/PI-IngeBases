using backend.Domain.Strategies;

namespace backend.Infraestructure.Strategies
{
    public class BiweeklyPaymentStrategy : IPaymentCalculationStrategy
    {
        public decimal CalculateGrossSalary(DateTime startDate, DateTime endDate, decimal baseSalary, int? hoursWorked = null)
        {
            int days = (endDate - startDate).Days + 1;
            return Math.Round(((baseSalary / 2) / 15m) * days, 3);
        }
    }
}
