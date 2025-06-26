namespace backend.Application.DTOs
{
    public class CompanyReportDto
    {
        public string Nombre { get; set; }
        public string FrecuenciaPago { get; set; }
        public string PeriodoPago { get; set; }
        public string FechaPago { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal CargasSociales { get; set; }
        public decimal Deducciones { get; set; }
        public decimal CostoEmpleador { get; set; }
    }
}