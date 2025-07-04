using backend.Application.Commands;
using backend.Domain;
using backend.Domain.Strategies;

namespace backend.Application.DeductionCalculation
{
    public class DeductionCalculationOrchestrator
    {
        private readonly List<IDeductionCalculationStrategy> _strategies;
        private readonly BenefitDeductionStrategy _benefitStrategy;
        private readonly IDisableBenefitForEmployeeCommand _disableBenefitForEmployeeCommand;

        public DeductionCalculationOrchestrator(
            CcssDeductionStrategy ccssStrategy,
            IncomeTaxDeductionStrategy incomeTaxStrategy,
            BenefitDeductionStrategy benefitStrategy,
            IDisableBenefitForEmployeeCommand disableBenefitForEmployeeCommand)
        {
            _strategies = new List<IDeductionCalculationStrategy>
            {
                ccssStrategy,
                incomeTaxStrategy
            };
            _benefitStrategy = benefitStrategy;
            _disableBenefitForEmployeeCommand = disableBenefitForEmployeeCommand;
        }

        public List<DeductionDetailModel> CalculateTotalDeductions(
            decimal grossSalary,
            string contractType,
            string gender,
            List<Benefit>? benefits,
            Guid paymentDetailsId,
            Guid? employeeId = null)
        {
            var details = ApplyMandatoryDeductions(grossSalary, contractType, gender, paymentDetailsId, out decimal totalAmountDeduced);

            // If there is not benefits or employeeId is null, return mandatory deductions only
            if (benefits == null || employeeId == null) return details;

            // If gross salary minus total mandatory deductions is less than 0,
            // disable all benefits for the employee and return mandatory deductions only
            if (grossSalary - totalAmountDeduced < 0)
            {
                DisableAllBenefitsForEmployee(benefits, employeeId.Value);
                return details;
            }

            ApplyVoluntaryDeductions(
                grossSalary,
                contractType,
                gender,
                benefits,
                paymentDetailsId,
                employeeId.Value,
                totalAmountDeduced,
                details
            );

            return details;
        }

        private List<DeductionDetailModel> ApplyMandatoryDeductions(
            decimal grossSalary,
            string contractType,
            string gender,
            Guid paymentDetailsId,
            out decimal totalAmountDeduced)
        {
            var details = new List<DeductionDetailModel>();
            totalAmountDeduced = 0m;

            foreach (var strategy in _strategies)
            {
            
                var amount = strategy.CalculateDeduction(
               grossSalary,
               contractType,
               gender,
               benefit: null,
               employeeId: null,
               paymentDetailsId: paymentDetailsId);

                if (amount > 0)
                {
                    totalAmountDeduced += amount;
                    details.Add(new DeductionDetailModel
                    {
                        Id = Guid.NewGuid(),
                        Name = strategy.GetType().Name.Replace("DeductionStrategy", ""),
                        AmountDeduced = amount,
                        PaymentDetailsId = paymentDetailsId,
                        DeductionType = "mandatory",
                        BenefitId = null
                    });
                }
            }
            return details;
        }

        private void DisableAllBenefitsForEmployee(List<Benefit> benefits, Guid employeeId)
        {
            foreach (var benefit in benefits)
            {
                _disableBenefitForEmployeeCommand.Execute(Guid.Parse(benefit.Id), employeeId);
            }
        }

        private void ApplyVoluntaryDeductions(
            decimal grossSalary,
            string contractType,
            string gender,
            List<Benefit> benefits,
            Guid paymentDetailsId,
            Guid employeeId,
            decimal totalAmountDeduced,
            List<DeductionDetailModel> details)
        {
            var benefitAmounts = benefits
                .Select(b => new
                {
                    Benefit = b,
                    Amount = _benefitStrategy.CalculateDeduction(grossSalary, contractType, gender, b, employeeId, paymentDetailsId)
                })
                .OrderBy(x => x.Amount)
                .ToList();

            decimal availableSalary = grossSalary - totalAmountDeduced;
            bool disableRest = false;

            foreach (var item in benefitAmounts)
            {
                if (disableRest)
                {
                    _disableBenefitForEmployeeCommand.Execute(Guid.Parse(item.Benefit.Id), employeeId);
                    continue;
                }

                if (availableSalary - item.Amount < 0)
                {
                    _disableBenefitForEmployeeCommand.Execute(Guid.Parse(item.Benefit.Id), employeeId);
                    disableRest = true;
                    continue;
                }

                availableSalary -= item.Amount;
                details.Add(new DeductionDetailModel
                {
                    Id = Guid.NewGuid(),
                    Name = item.Benefit.Name,
                    AmountDeduced = item.Amount,
                    PaymentDetailsId = paymentDetailsId,
                    DeductionType = "voluntary",
                    BenefitId = Guid.Parse(item.Benefit.Id)
                });

                if (item.Benefit.IsDeleted)
                {
                    _disableBenefitForEmployeeCommand.Execute(Guid.Parse(item.Benefit.Id), employeeId);
                    continue;
                }
            }
        }
    }
}