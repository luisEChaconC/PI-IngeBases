using System.ComponentModel.DataAnnotations;

namespace API_Registro_Nacional.Models
{
    public class LegalIDRequest
    {
        [Required(ErrorMessage = "El campo LegalID es requerido")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo LegalID debe tener exactamente 10 dígitos")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El campo LegalID debe contener solo dígitos")]
        public string LegalID { get; set; }
    }
}