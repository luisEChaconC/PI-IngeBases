using System.ComponentModel.DataAnnotations;

namespace backend.Domain
{
    public class PayslipModel
    {
        public string? Id { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "El tipo de periodo es obligatorio.")]
        [RegularExpression(@"^(SEMANAL|QUINCENAL|MENSUAL)$", ErrorMessage = "El tipo de periodo debe ser SEMANAL, QUINCENAL o MENSUAL.")]
        public string PeriodType { get; set; }

        [Required(ErrorMessage = "El rango de fechas es obligatorio.")]
        public string DateRange { get; set; }

        [Required(ErrorMessage = "El mes de pago es obligatorio.")]
        public string PaymentMonth { get; set; }

        [Required]
        [Range(0, 100000000, ErrorMessage = "El salario bruto debe ser un valor positivo.")]
        public decimal GrossSalary { get; set; }

        [Required]
        [Range(0, 100000000, ErrorMessage = "El pago neto debe ser un valor positivo.")]
        public decimal NetPay { get; set; }

        public List<PayslipItem> Items { get; set; } = new();
    }

    public class PayslipItem
    {
        [Required]
        public string Label { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public bool IsBold { get; set; } = false;
    }
}
