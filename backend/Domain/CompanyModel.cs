using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class CompanyModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(80, ErrorMessage = "Name cannot exceed 80 characters.")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "PaymentType is required.")]
        [RegularExpression("^(Monthly|Biweekly|Weekly)$", ErrorMessage = "PaymentType must be 'Monthly', 'Biweekly', or 'Weekly'.")]
        public string PaymentType { get; set; }

        public List<EmployeeModel> ?Employees { get; set; }
        
        public int EmployeeCount { get; set; }
        
        public string? LegalId { get; set; }

        public int? MaxBenefitsPerEmployee { get; set; }

        [Required(ErrorMessage = "CreationDate is required.")]
        public DateTime CreationDate { get; set; }

        [StringLength(80, ErrorMessage = "CreationAuthor cannot exceed 80 characters.")]
        public string? CreationAuthor { get; set; }

        public DateTime? LastModificationDate { get; set; }

        [StringLength(80, ErrorMessage = "LastModificationAuthor cannot exceed 80 characters.")]
        public string? LastModificationAuthor { get; set; }

        public PersonsModel? Person { get; set; }

        public List<ContactModel> ?Contact { get; set; }

        public List<dynamic> ?EmployeesDynamic { get; set; }

        public CompanyModel()
        {
            Id = string.Empty;
            Name = string.Empty;
            Description = null;
            PaymentType = string.Empty;
            MaxBenefitsPerEmployee = null;
            CreationDate = DateTime.Now;
            CreationAuthor = null;
            LastModificationDate = DateTime.Now;
            LastModificationAuthor = null;
            Person = null;
            Contact = null;
            Employees = null;
            EmployeesDynamic = null;
        }
        public CompanyModel(string id, string name, string? description, string paymentType, List<EmployeeModel> employees, int? maxBenefitsPerEmployee, DateTime creationDate, string? creationAuthor, DateTime? lastModificationDate, string? lastModificationAuthor)
        {
            Id = id;
            Name = name;
            Description = description;
            PaymentType = paymentType;
            Employees = employees;
            MaxBenefitsPerEmployee = maxBenefitsPerEmployee;
            CreationDate = creationDate;
            CreationAuthor = creationAuthor;
            LastModificationDate = lastModificationDate;
            LastModificationAuthor = lastModificationAuthor;
        }
    }
}