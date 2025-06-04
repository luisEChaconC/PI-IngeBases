using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class NaturalPersonModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(80, ErrorMessage = "FirstName cannot exceed 80 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "FirstSurname is required.")]
        [StringLength(80, ErrorMessage = "FirstSurname cannot exceed 80 characters.")]
        public string FirstSurname { get; set; }

        [Required(ErrorMessage = "SecondSurname is required.")]
        [StringLength(80, ErrorMessage = "SecondSurname cannot exceed 80 characters.")]
        public string SecondSurname { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(1, ErrorMessage = "Gender must be a single character.")]
        [RegularExpression("^[FM]$", ErrorMessage = "Gender must be 'F' or 'M'.")]
        public string Gender { get; set; }

        public string? UserId { get; set; }

        
        

        public NaturalPersonModel()
        {
            Id = string.Empty;
            FirstName = string.Empty;
            FirstSurname = string.Empty;
            SecondSurname = string.Empty;
            UserId = null;
            Gender = "M"; 
        }

        public NaturalPersonModel(string id, string firstName, string firstSurname, string secondSurname, string? userId, string gender)
        {
            Id = id;
            FirstName = firstName;
            FirstSurname = firstSurname;
            SecondSurname = secondSurname;
            UserId = userId;
            Gender = gender;
        }
    }
}