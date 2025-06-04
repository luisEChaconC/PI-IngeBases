using System;

namespace backend.Domain
{
    public class EmployeeGetIDModel
    {
        public string Id { get; set; }
        public string WorkerId { get; set; }
        public string CompanyId { get; set; }
        public DateTime EmployeeStartDate { get; set; }
        public string ContractType { get; set; }
        public decimal GrossSalary { get; set; }
        public bool HasToReportHours { get; set; }

        public string FirstName { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
    }
}
