using backend.Domain.Enums;

namespace backend.Application.DTOs
{
    public class CalculateGrossPaymentDto
    {
        public EmployeeTypePayment EmployeeTypePayment { get; set; }
        public decimal BaseSalary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? WorkedHours { get; set; }
    }
}