using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "WorkerId is required.")]
        [StringLength(80, ErrorMessage = "WorkerId cannot exceed 80 characters.")]
        public string WorkerId { get; set; }

        [Required(ErrorMessage = "CompanyId is required.")]
        public Guid CompanyId { get; set; }

        [Required(ErrorMessage = "EmployeeStartDate is required.")]
        public DateTime EmployeeStartDate { get; set; }

        [Required(ErrorMessage = "ContractType is required.")]
        [RegularExpression("^(Full-Time|Part-Time|Professional Services|Hourly)$", ErrorMessage = "ContractType must be 'Full-Time', 'Part-Time', 'Professional Services', or 'Hourly'.")]
        public string ContractType { get; set; }

        [Required(ErrorMessage = "GrossSalary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "GrossSalary must be a positive value.")]
        public decimal GrossSalary { get; set; }

        [Required(ErrorMessage = "HasToReportHours is required.")]
        public bool HasToReportHours { get; set; }

        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; } = false;


        public EmployeeModel()
        {
            Id = Guid.Empty;
            WorkerId = string.Empty;
            CompanyId = Guid.Empty;
            EmployeeStartDate = DateTime.Now;
            ContractType = string.Empty;
            GrossSalary = 0.0m;
            HasToReportHours = false;
        }

        public EmployeeModel(Guid id, string workerId, Guid companyId, DateTime employeeStartDate, string contractType, decimal grossSalary, bool hasToReportHours)
        {
            Id = id;
            WorkerId = workerId;
            CompanyId = companyId;
            EmployeeStartDate = employeeStartDate;
            ContractType = contractType;
            GrossSalary = grossSalary;
            HasToReportHours = hasToReportHours;
        }
    }
}