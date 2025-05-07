using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_Registro_Nacional.Helpers;
using API_Registro_Nacional.Models;

namespace API_Registro_Nacional.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NationalRegisterController : ControllerBase
    {
        [HttpPost("validate")]
        public IActionResult ValidateLegalID([FromBody] LegalIDRequest request)
        {
            if (ModelState.IsValid) {
                bool isValid = LegalIDValidator.IsLegalIDValid(request.LegalID);
                return Ok(isValid);
            }
            return BadRequest(ModelState);
        }
    }
}
