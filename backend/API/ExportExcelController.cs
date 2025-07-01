using backend.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace backend.API
{
    [ApiController]
    [Route("api/report")]
    public class ExportExcelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExportExcelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("excel")]
        public async Task<IActionResult> ExportToExcel([FromBody] ExportExcelDto command)
        {
            var fileContent = await _mediator.Send(command);
            var fileName = $"{command.SheetName ?? "Reporte"}.xlsx"; // Change name if needed

            return File(fileContent,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }
    }

}
