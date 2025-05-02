using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController()
        {
            _companyService = new CompanyService();
        }
        [HttpGet]
        public List<CompanyViewModel> Get()
        {
            return _companyService.GetCompanies();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateCompany(Company company)
        {
            try
            {
                if (company == null)
                    return BadRequest("Company is null");

                var result = _companyService.CreateCompany(company);
                return new JsonResult(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating company");
            }
        }

    }
}
