using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Registro_Nacional.Helpers;

namespace API_Registro_Nacional.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NationalRegisterController : ControllerBase
    {
        [HttpGet("validate/{legalID}")]
        public IActionResult ValidateLegalID(string legalID)
        {
            bool isValid = LegalIDValidator.IsLegalIDValid(legalID);
            return Ok(isValid);
        }
    }
}
