using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class EmployerModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "WorkerId is required.")]
        [StringLength(80, ErrorMessage = "WorkerId cannot exceed 80 characters.")]
        public string WorkerId { get; set; }

        [Required(ErrorMessage = "CompanyId is required.")]
        public string CompanyId { get; set; }

        public EmployerModel()
        {
            Id = string.Empty;
            WorkerId = string.Empty;
            CompanyId = string.Empty;
        }

        public EmployerModel(string id, string workerId, string companyId)
        {
            Id = id;
            WorkerId = workerId;
            CompanyId = companyId;
        }
    }
}