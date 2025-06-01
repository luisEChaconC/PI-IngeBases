using backend.Domain;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly APIRepository _repo;

        public APIController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _repo = new APIRepository(connectionString);
        }
        [HttpGet]
        public ActionResult<List<ApiModel>> GetAPIs()
        {
            var apis = _repo.GetAPIs();
            return Ok(apis);
        }

        [HttpGet("{apiId}/parameters")]
        public ActionResult<List<ApiParameterModel>> GetParameters(Guid apiId)
        {
            var parameters = _repo.GetParametersByAPI(apiId);
            return Ok(parameters);
        }

        [HttpGet("parameters/{parameterId}/values")]
        public ActionResult<List<ParameterValueModel>> GetParameterValues(Guid parameterId)
        {
            var values = _repo.GetParameterValues(parameterId);
            return Ok(values);
        }

        [HttpPost("parameters/values")]
        public ActionResult<bool> AddParameterValue([FromBody] ParameterValueModel value)
        {
            if (value == null)
                return BadRequest("Parameter value is null");

            value.Id = Guid.NewGuid();
            var result = _repo.AddParameterValue(value);
            return Ok(result);
        }
    }
}