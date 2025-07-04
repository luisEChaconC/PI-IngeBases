using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class ContactModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [RegularExpression("^(Phone Number|Email)$", ErrorMessage = "Type must be either 'Phone Number' or 'Email'.")]
        public string Type { get; set; }

        [StringLength(25, ErrorMessage = "PhoneNumber cannot exceed 25 characters.")]
        public string? PhoneNumber { get; set; }

        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string? Email { get; set; }

        public string PersonId { get; set; }

        public ContactModel()
        {
            Id = string.Empty;
            Type = string.Empty;
            PhoneNumber = null;
            Email = null;
            PersonId = string.Empty;
        }

        public ContactModel(string id, string type, string? phoneNumber, string? email, string personId)
        {
            Id = id;
            Type = type;
            PhoneNumber = phoneNumber;
            Email = email;
            PersonId = personId;
        }
    }
}