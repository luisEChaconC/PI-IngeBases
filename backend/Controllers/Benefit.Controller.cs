using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitController : ControllerBase
    {
        private readonly BenefitService _benefitService;

        public BenefitController()
        {
            _benefitService = new BenefitService();
        }

        [HttpGet]
        public List<Benefit> Get()
        {
            return _benefitService.GetBenefits();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateBenefit(Benefit benefit)
        {
            try
            {
                if (benefit == null)
                    return BadRequest("Benefit is null");

                var result = _benefitService.CreateBenefit(benefit);
                return new JsonResult(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating benefit");
            }
        }
    }
}
