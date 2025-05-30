using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class UpdateEmployeeModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        // NaturalPersons
        public string? FirstName { get; set; }
        public string? FirstSurname { get; set; }
        public string? SecondSurname { get; set; }

        // Persons
        public string? LegalId { get; set; }

        // Employees
        public string? WorkerId { get; set; }
        public string? ContractType { get; set; } 
        public decimal? GrossSalary { get; set; }

        // Contacts
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
