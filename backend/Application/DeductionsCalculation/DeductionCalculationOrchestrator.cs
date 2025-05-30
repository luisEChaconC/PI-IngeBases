using backend.Domain;
using backend.Domain.Strategies;
using System.Collections.Generic;

namespace backend.Application.DeductionCalculation
{
    public class DeductionCalculationOrchestrator
    {
        private readonly List<IDeductionCalculationStrategy> _strategies;
        private readonly BenefitDeductionStrategy _benefitStrategy;

        public DeductionCalculationOrchestrator(
            CcssDeductionStrategy ccssStrategy,
            IncomeTaxDeductionStrategy incomeTaxStrategy,
            BenefitDeductionStrategy benefitStrategy)
        {
            _strategies = new List<IDeductionCalculationStrategy>
            {
                ccssStrategy,
                incomeTaxStrategy
            };
            _benefitStrategy = benefitStrategy;
        }

        public decimal CalculateTotalDeductions(Guid employee, decimal grossSalary, List<Benefit>? benefits)
        {
            decimal total = 0;

            foreach (var strategy in _strategies)
            {
                total += strategy.CalculateDeduction(employee, grossSalary);
            }

           /* foreach (var benefit in benefits)
            {
                total += _benefitStrategy.CalculateDeduction(employee, grossSalary, benefit);
            }*/

            return total;
        }
    }
}