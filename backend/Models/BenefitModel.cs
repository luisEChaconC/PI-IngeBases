using backend.Models;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace backend.Models
{
    public class Benefit
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 35 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Solo letras, tildes y ñ son permitidas.")]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [RegularExpression(@"^(API|FixedAmount|FixedPercentage)$", ErrorMessage = "Tipo debe ser API, Monto Fijo o Porcentaje Fijo.")]
        public string Type { get; set; }

        public string? LinkAPI { get; set; }

        [Range(1, 100, ErrorMessage = "El porcentaje fijo debe estar entre 1 y 100.")]
        public int? FixedPercentage { get; set; }

        [Range(1, 10000000, ErrorMessage = "El monto fijo debe estar entre 1 y 10,000,000.")]
        public int? FixedAmount { get; set; }

        [Range(0, 480, ErrorMessage = "El tiempo mínimo debe estar entre 0 y 480 meses.")]
        public int RequiredMonthsWorked { get; set; }

        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un tipo de empleado.")]
        [EligibleEmployeeTypesValidation]
        public List<string> EligibleEmployeeTypes { get; set; }
    }

    public class EligibleEmployeeTypesValidationAttribute : ValidationAttribute
    {
        private readonly string[] allowed = new[] { "Full-Time", "Part-Time", "Professional Services", "Hourly" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is List<string> list && list.All(type => allowed.Contains(type)))
                return ValidationResult.Success;

            return new ValidationResult("Tipos de empleado elegibles inválidos.");
        }
    }
}


