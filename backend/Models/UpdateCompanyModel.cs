using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class UpdateCompanyModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(80, ErrorMessage = "Name cannot exceed 80 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "PaymentType is required.")]
        [RegularExpression("^(Monthly|Biweekly|Weekly)$", ErrorMessage = "PaymentType must be 'Monthly', 'Biweekly', or 'Weekly'.")]
        public string PaymentType { get; set; }

        public int EmployeeCount { get; set; }
        
        public int? MaxBenefits { get; set; }

        public PersonData? Person { get; set; }
        
        public ContactData? Contact { get; set; }

        public UpdateCompanyModel()
        {
            Id = string.Empty;
            Name = string.Empty;
            EmployeeCount = 0;
            PaymentType = string.Empty;
            MaxBenefits = null;
            Person = new PersonData();
            Contact = new ContactData();
        }
    }

    public class PersonData
    {
        public string? Province { get; set; }
        public string? Canton { get; set; }
        public string? Neighborhood { get; set; }
        public string? AdditionalDirectionDetails { get; set; }
        public string? LegalId { get; set; }
    }

    public class ContactData
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
