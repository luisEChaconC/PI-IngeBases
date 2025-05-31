using backend.Application.DTOs;
using backend.Domain;

public class CalculateDeductionDto
{
    public CalculateGrossPaymentDto GrossPayment { get; set; }
    public List<Benefit> Benefits { get; set; }
    public Guid PaymentDetailsId { get; set; }
}