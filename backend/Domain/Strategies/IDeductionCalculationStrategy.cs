namespace backend.Domain.Strategies
{
    public interface IDeductionCalculationStrategy
    {
        decimal CalculateDeduction(decimal grossSalary, string contractType, string gender, Benefit? benefit = null, Guid? employeeId = null, Guid? paymentDetailsId = null);
    }
}