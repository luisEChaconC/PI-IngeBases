using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class PersonsModel
    {
        public string Id { get; set; }

        public string LegalId { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [RegularExpression("^(Legal Entity|Natural Person)$", ErrorMessage = "Type must be either 'Legal Entity' or 'Natural Person'.")]
        public string Type { get; set; }

        [StringLength(50, ErrorMessage = "Province cannot exceed 50 characters.")]
        public string? Province { get; set; }

        [StringLength(50, ErrorMessage = "Canton cannot exceed 50 characters.")]
        public string? Canton { get; set; }

        [StringLength(80, ErrorMessage = "Neighborhood cannot exceed 80 characters.")]
        public string? Neighborhood { get; set; }

        [StringLength(150, ErrorMessage = "AdditionalDirectionDetails cannot exceed 150 characters.")]
        public string? AdditionalDirectionDetails { get; set; }

        public PersonsModel()
        {
            Id = string.Empty;
            LegalId = string.Empty;
            Type = string.Empty;
            Province = null;
            Canton = null;
            Neighborhood = null;
            AdditionalDirectionDetails = null;
        }

        public PersonsModel(string id, string legalId, string type, string? province, string? canton, string? neighborhood, string? additionalDirectionDetails)
        {
            Id = id;
            LegalId = legalId;
            Type = type;
            Province = province;
            Canton = canton;
            Neighborhood = neighborhood;
            AdditionalDirectionDetails = additionalDirectionDetails;
        }
    }
}