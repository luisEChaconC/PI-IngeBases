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

        [HttpGet("{id}")]
        public ActionResult<Benefit> GetById(Guid id)
        {
            var benefits = _benefitService.GetBenefits();
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
    }
}
