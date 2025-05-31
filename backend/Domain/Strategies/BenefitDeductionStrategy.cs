namespace backend.Domain.Strategies
{
    public class BenefitDeductionStrategy : IDeductionCalculationStrategy
    {
        public decimal CalculateDeduction(decimal grossSalary, Benefit? benefit = null)
        {
            if (benefit == null) return 0;

            switch (benefit.Type)
            {
                case "FixedAmount":
                    return benefit.FixedAmount ?? 0;
                case "FixedPercentage":
                    return Math.Round(grossSalary * ((benefit.FixedPercentage ?? 0) / 100m), 2);
                case "API":
             
                    return CallBenefitApi(benefit);
                default:
                    return 0;
            }
        }

        private decimal CallBenefitApi(Benefit benefit)
        {
            // TODO: implementar llamada a API
            return 0;
        }
    }
}