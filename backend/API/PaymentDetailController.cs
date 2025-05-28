
using backend.Application.Commands.PaymentDetails;
using backend.Application.Queries.PaymentDetails;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentDetailController : ControllerBase
    {
        private readonly ICreatePaymentDetailCommand _createCommand;
        private readonly IGetPaymentDetailByIdQuery _getByIdQuery;
        private readonly IGetPaymentDetailsByEmployeeIdQuery _getByEmployeeIdQuery;
        private readonly IGetPaymentDetailsByCompanyIdQuery _getByCompanyIdQuery;

        public PaymentDetailController(
            ICreatePaymentDetailCommand createCommand,
            IGetPaymentDetailByIdQuery getByIdQuery,
            IGetPaymentDetailsByEmployeeIdQuery getByEmployeeIdQuery,
            IGetPaymentDetailsByCompanyIdQuery getByCompanyIdQuery)
        {
            _createCommand = createCommand;
            _getByIdQuery = getByIdQuery;
            _getByEmployeeIdQuery = getByEmployeeIdQuery;
            _getByCompanyIdQuery = getByCompanyIdQuery;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentDetailModel model)
        {
            var id = await _createCommand.ExecuteAsync(model);
            return Ok(new { Id = id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var detail = await _getByIdQuery.ExecuteAsync(id);
            if (detail == null) return NotFound();
            return Ok(detail);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(Guid employeeId)
        {
            var details = await _getByEmployeeIdQuery.ExecuteAsync(employeeId);
            return Ok(details);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompanyId(Guid companyId)
        {
            var details = await _getByCompanyIdQuery.ExecuteAsync(companyId);
            return Ok(details);
        }
    }
}