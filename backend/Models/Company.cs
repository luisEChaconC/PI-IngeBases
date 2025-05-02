namespace backend.Models
{
    public class Company // This class represents all the attributes from the table Company
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PaymentType { get; set; }
        public int MaxBenefitsPerEmployee { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationAuthor { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public string? LastModificationAuthor { get; set; }
    }

    public class CompanyViewModel  // For the view company information
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PaymentType { get; set; }
        public string Province { get; set; }
        public string Canton { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string AdditionalDirectionDetails { get; set; }
        public string LegalId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}

