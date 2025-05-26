using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class UpdateCompanyModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(80, ErrorMessage = "Name cannot exceed 80 characters.")]
        public string Name { get; set; }

        public string? LegalId { get; set; }
        
        [Required(ErrorMessage = "PaymentType is required.")]
        [RegularExpression("^(Monthly|Biweekly|Weekly)$", ErrorMessage = "PaymentType must be 'Monthly', 'Biweekly', or 'Weekly'.")]
        public string PaymentType { get; set; }

        public UpdateCompanyModel()
        {
            Id = string.Empty;
            Name = string.Empty;
            LegalId = null;
            PaymentType = string.Empty;
        }
        
    }
}