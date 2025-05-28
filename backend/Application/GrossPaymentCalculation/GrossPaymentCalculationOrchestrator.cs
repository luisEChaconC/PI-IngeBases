using backend.Domain.Enums;
using backend.Domain.Strategies;

namespace backend.Application.GrossPaymentCalculation
{
    public class GrossPaymentCalculationOrchestrator
    {
        private readonly Dictionary<EmployeeTypePayment, IPaymentCalculationStrategy> _strategies;

        public GrossPaymentCalculationOrchestrator(
            MonthlyPaymentStrategy monthlyStrategy,
            BiweeklyPaymentStrategy biweeklyStrategy,
            WeeklyPaymentStrategy weeklyStrategy)
        {
            _strategies = new Dictionary<EmployeeTypePayment, IPaymentCalculationStrategy>
            {
                { EmployeeTypePayment.Monthly, monthlyStrategy },
                { EmployeeTypePayment.Biweekly, biweeklyStrategy },
                { EmployeeTypePayment.Weekly, weeklyStrategy }
            };
        }

        public decimal CalculateGrossPayment(EmployeeTypePayment type, decimal baseSalary, DateTime startDate, DateTime endDate, int? workedHours = null)
        {
            if (!_strategies.TryGetValue(type, out var strategy))
            {
                throw new ArgumentException($"No strategy found for employee type: {type}");
            }

            return strategy.CalculateGrossPayment(startDate, endDate,baseSalary, workedHours);
        }
    }
}

