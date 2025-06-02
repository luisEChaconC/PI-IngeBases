using backend.Application.DTOs;
using backend.Domain;

public class CalculateDeductionDto
{
    public Guid EmployeeId { get; set; }
    public Decimal GrossSalary { get; set; }
    public Guid PaymentDetailsId { get; set; }
}