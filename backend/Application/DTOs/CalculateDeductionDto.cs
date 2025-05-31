using backend.Application.DTOs;

public class CalculateDeductionDto
{
    public Guid EmployeeId { get; set; }
    public CalculateGrossPaymentDto GrossPayment { get; set; }
}