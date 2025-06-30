using backend.Application.Queries;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerCostController : ControllerBase
    {
        private readonly IGetEmployerCostQuery _getEmployerCostQuery;

        public EmployerCostController(IGetEmployerCostQuery getEmployerCostQuery)
        {
            _getEmployerCostQuery = getEmployerCostQuery;
        }

        [HttpGet("GetByPayrollId/{payrollId}")]
        public IActionResult GetByPayrollId(Guid payrollId)
        {
            try
            {
                var cost = _getEmployerCostQuery.Execute(payrollId);
                if (cost == null)
                    return NotFound(new { message = "No se encontró información de costo patronal para esta planilla." });

                return Ok(cost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el costo patronal.", error = ex.Message });
            }
        }
    }
}
