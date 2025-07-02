using backend.Application.Payslip.Queries;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayslipController : ControllerBase
    {
        private readonly GetPayslipsByEmployeeIdQuery _getPayslipsByEmployeeIdQuery;
        private readonly GetPayslipByEmployeeIdAndStartDateQuery _getPayslipByEmployeeIdAndStartDateQuery;

        public PayslipController(
            GetPayslipsByEmployeeIdQuery getPayslipsByEmployeeIdQuery,
            GetPayslipByEmployeeIdAndStartDateQuery getPayslipByEmployeeIdAndStartDateQuery)
        {
            _getPayslipsByEmployeeIdQuery = getPayslipsByEmployeeIdQuery;
            _getPayslipByEmployeeIdAndStartDateQuery = getPayslipByEmployeeIdAndStartDateQuery;
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<List<PayslipModel>>> GetByEmployeeId(Guid employeeId)
        {
            try
            {
                var result = await _getPayslipsByEmployeeIdQuery.ExecuteAsync(employeeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener colillas: {ex.Message}");
            }
        }

        [HttpGet("employee/{employeeId}/by-start-date")]
        public async Task<ActionResult<PayslipModel>> GetByEmployeeIdAndStartDate(Guid employeeId, [FromQuery] DateTime startDate)
        {
            try
            {
                var result = await _getPayslipByEmployeeIdAndStartDateQuery.ExecuteAsync(employeeId, startDate);

                if (result == null)
                    return NotFound("No se encontró la colilla para ese periodo.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener colilla: {ex.Message}");
            }
        }
    }
}
