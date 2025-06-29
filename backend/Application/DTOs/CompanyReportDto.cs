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
        public decimal DeduccionesVoluntarias { get; set; }
        public decimal CostoEmpleador { get; set; }


        public TableReportDto MapToTableReportDto(List<CompanyReportDto> companyReports)
        {
            var columns = new List<string>
            {
                "Nombre", "FrecuenciaPago", "PeriodoPago", "FechaPago",
                "SalarioBruto", "CargasSociales", "DeduccionesVoluntarias", "CostoEmpleador"
            };

            var rows = companyReports.Select(r => new List<object>
            {
                r.Nombre, r.FrecuenciaPago, r.PeriodoPago, r.FechaPago, r.SalarioBruto, r.CargasSociales, r.DeduccionesVoluntarias, r.CostoEmpleador
            }).ToList();

            return new TableReportDto
            {
                Columns = columns,
                Rows = rows,
                SheetName = "Reporte de Historial de Pagos por Empresa"
            };
        }

    }
}