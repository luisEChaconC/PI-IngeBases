namespace backend.Application.DTOs
{
    public class EmployerEmployeePayrollReportDto
    {
        public string? CompanyName { get; set; }
        public string? EmployerName { get; set; }
        public List<EmployeePayrollInfoDto> Employees { get; set; } = new();
    }

    public class EmployeePayrollInfoDto
    {
        public string? EmployeeName { get; set; }
        public string? LegalId { get; set; }
        public string? EmployeeType { get; set; }
        public string? PaymentPeriod { get; set; }
        public string? PaymentDate { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal EmployerSocialCharges { get; set; }
        public decimal VoluntaryDeductions { get; set; }
        public decimal EmployerCost { get; set; }
    }
}