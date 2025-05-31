using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class PaymentDetailModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo EmployeeId es obligatorio.")]
        public Guid EmployeeId { get; set; }

        public Guid? PayrollId { get; set; }


        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "GrossSalary must be positive.")]
        public decimal GrossSalary { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }
    }
}
