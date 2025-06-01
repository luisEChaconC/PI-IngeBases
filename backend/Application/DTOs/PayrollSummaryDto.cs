namespace backend.Application.DTOs
{
    public class PayrollSummaryDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PayrollManagerFullName { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalAmountDeducted { get; set; }
    }
}