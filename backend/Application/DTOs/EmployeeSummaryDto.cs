namespace backend.Application.DTOs
{
    public class EmployeeSummaryDto
    {
        public Guid Id { get; set; }
        public string WorkerId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime EmployeeStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ContractType { get; set; }
        public decimal GrossSalary { get; set; }
        public bool HasToReportHours { get; set; }
        public string FirstName { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public string? UserId { get; set; }
        public string Gender { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}