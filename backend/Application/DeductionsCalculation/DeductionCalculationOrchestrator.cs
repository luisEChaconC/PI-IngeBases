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

        public List<DeductionDetailModel> CalculateTotalDeductions(decimal grossSalary, string contractType, List<Benefit>? benefits, Guid paymentDetailsId)
        {
            var details = new List<DeductionDetailModel>();

            foreach (var strategy in _strategies)
            {
                var amount = strategy.CalculateDeduction(grossSalary, contractType);
                if (amount > 0)
                {
                    details.Add(new DeductionDetailModel
                    {
                        Id = Guid.NewGuid(),
                        Name = strategy.GetType().Name.Replace("DeductionStrategy", ""),
                        AmountDeduced = amount,
                        PaymentDetailsId = paymentDetailsId,
                        DeductionType = "mandatory"
                    });
            }
            }

            if (benefits != null)
            {
            foreach (var benefit in benefits)
            {
                    var amount = _benefitStrategy.CalculateDeduction(grossSalary,contractType, benefit);
                    if (amount > 0)
                    {
                        details.Add(new DeductionDetailModel
                        {
                            Id = Guid.NewGuid(),
                            Name = benefit.Name,
                            AmountDeduced = amount,
                            PaymentDetailsId = paymentDetailsId,
                            DeductionType = "voluntary"
                        });
                    }
                }
            }

            return details;
        }
    }
}