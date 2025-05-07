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

        [HttpGet("{id}")]
        public ActionResult<Benefit> GetById(Guid id, [FromQuery] string companyId)
        {
            var benefits = _benefitService.GetBenefits(companyId);
            var benefit = benefits.FirstOrDefault(b => b.Id == id.ToString());

            if (benefit == null)
                return NotFound();

            return Ok(benefit);
        }

        [HttpPost]
        public ActionResult<bool> CreateBenefit(Benefit benefit)
        {
            try
            {
                if (benefit == null)
                    return BadRequest("Benefit is null");

                var result = _benefitService.CreateBenefit(benefit); 
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error creating benefit: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<List<Benefit>> Get([FromQuery] string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return BadRequest("Se requiere el ID de la empresa.");

            var benefits = _benefitService.GetBenefits(companyId);
            return Ok(benefits);
        }

    }
}
