using backend.Application.DTOs;
using backend.Domain;

public class CalculateDeductionDto
{
    public Guid EmployeeId { get; set; }
    public CalculateGrossPaymentDto GrossPayment { get; set; }
    public Guid PaymentDetailsId { get; set; }
}