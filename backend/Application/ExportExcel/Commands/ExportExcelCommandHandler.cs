using ClosedXML.Excel;
using MediatR;
using backend.Application.DTOs;

public class ExportExcelCommandHandler : IRequestHandler<ExportExcelDto, byte[]>
{
    public Task<byte[]> Handle(ExportExcelDto request, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(request.SheetName ?? "Reporte");

        // Encabezados
        for (int i = 0; i < request.Columns.Count; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = request.Columns[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        // Filas de datos
        for (int row = 0; row < request.Rows.Count; row++)
        {
            for (int col = 0; col < request.Columns.Count; col++)
            {
                var cell = worksheet.Cell(row + 2, col + 1);
                var value = request.Rows[row][col];

                switch (value)
                {
                    case DateTime dt:
                        cell.Value = dt;
                        cell.Style.DateFormat.Format = "dd/MM/yyyy";
                        break;

                    case decimal dec:
                        cell.Value = dec;
                        cell.Style.NumberFormat.Format = "₡ #,##0";
                        break;

                    case double d:
                        cell.Value = d;
                        cell.Style.NumberFormat.Format = "₡ #,##0.00";
                        break;

                    case int i:
                        cell.Value = i;
                        break;

                    default:
                        cell.Value = value?.ToString();
                        break;
                }
            }
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Task.FromResult(stream.ToArray());
    }
}
