using NUnit.Framework;
using backend.Application.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;

public class ExportExcelCommandHandlerTests
{
    [Test]
    public async Task ExportExcelCommandHandler_GeneratesValidExcelFile()
    {
        // Arrange
        var handler = new ExportExcelCommandHandler();

        var command = new ExportExcelDto
        {
            SheetName = "TestSheet",
            Columns = new List<string> { "Nombre", "Salario", "Deducciones" },
            Rows = new List<List<object>>
            {
                new List<object> { "Rigo Vega", 1000, 200 },
                new List<object> { "Luis", 1200, 300 }
            }
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0, "El archivo Excel no debe estar vacío.");

        // bytes de un archivo OpenXML (PKZIP)
        Assert.AreEqual('P', (char)result[0]);
        Assert.AreEqual('K', (char)result[1]);
    }
    [Test]
    public async Task ExportExcelCommandHandler_ContainsExpectedData()
    {
        // Arrange
        var handler = new ExportExcelCommandHandler();
        var command = new ExportExcelDto
        {
            SheetName = "TestSheet",
            Columns = new List<string> { "Nombre", "Salario" },
            Rows = new List<List<object>>
        {
            new List<object> { "Rigo", 1000 }
        }
        };

        // Act
        var file = await handler.Handle(command, CancellationToken.None);

        // Assert
        using var stream = new MemoryStream(file);
        using var workbook = new XLWorkbook(stream);
        var sheet = workbook.Worksheet("TestSheet");

        Assert.AreEqual("Nombre", sheet.Cell(1, 1).Value);
        Assert.AreEqual("Salario", sheet.Cell(1, 2).Value);
        Assert.AreEqual("Rigo", sheet.Cell(2, 1).Value);
        Assert.AreEqual(1000.0, sheet.Cell(2, 2).GetDouble());
    }
    [Test]
    public async Task ExportExcelCommandHandler_GeneratesProperFormats()
    {
        // Arrange
        var handler = new ExportExcelCommandHandler();

        var date = new DateTime(2024, 12, 25);

        var command = new ExportExcelDto
        {
            SheetName = "PruebaTipos",
            Columns = new List<string> { "Nombre", "FechaIngreso", "SalarioBruto", "AporteVoluntario", "Edad" },
            Rows = new List<List<object>>
            {
                new List<object>
                {
                    "Rigo Vega",        // string
                    date,              // DateTime
                    123456.00m,        // decimal (colones)
                    456.78,            // double (colones con centavos)
                    34                 // int
                }
            }
        };

        // Act
        var fileBytes = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(fileBytes);
        Assert.Greater(fileBytes.Length, 0);

        using var stream = new MemoryStream(fileBytes);
        using var workbook = new XLWorkbook(stream);
        var sheet = workbook.Worksheet("PruebaTipos");

        Assert.AreEqual("Nombre", sheet.Cell(1, 1).GetString());
        Assert.AreEqual("FechaIngreso", sheet.Cell(1, 2).GetString());
        Assert.AreEqual("SalarioBruto", sheet.Cell(1, 3).GetString());
        Assert.AreEqual("AporteVoluntario", sheet.Cell(1, 4).GetString());
        Assert.AreEqual("Edad", sheet.Cell(1, 5).GetString());

        Assert.AreEqual("Rigo Vega", sheet.Cell(2, 1).GetString());
        Assert.AreEqual(date, sheet.Cell(2, 2).GetDateTime());
        Assert.AreEqual(123456.00, sheet.Cell(2, 3).GetDouble());
        Assert.AreEqual(456.78, sheet.Cell(2, 4).GetDouble(), 0.001);
        Assert.AreEqual(34, sheet.Cell(2, 5).GetDouble());

        Assert.AreEqual("dd/MM/yyyy", sheet.Cell(2, 2).Style.DateFormat.Format);
        Assert.AreEqual("₡ #,##0", sheet.Cell(2, 3).Style.NumberFormat.Format);
        Assert.AreEqual("₡ #,##0.00", sheet.Cell(2, 4).Style.NumberFormat.Format);
    }
}
