using backend.Application.DTOs;
using backend.Domain;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyReportController : ControllerBase
    {
        private readonly IGetCompanyReportsQuery _getCompanyReportsQuery;
        public CompanyReportController(IGetCompanyReportsQuery getCompanyReportsQuery)
        {
            _getCompanyReportsQuery = getCompanyReportsQuery;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var reports = await _getCompanyReportsQuery.ExecuteAsync(startDate, endDate);

                var tableReport = new CompanyReportDto().MapToTableReportDto(reports);

                return Ok(tableReport);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al obtener el reporte de historial de pagos por empresa.", error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var reports = await _getCompanyReportsQuery.ExecuteAllAsync();
                var tableReport = new CompanyReportDto().MapToTableReportDto(reports);
                return Ok(tableReport);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al obtener todos los reportes de historial de pagos por empresa.", error = ex.Message });
            }
        }
    }
}