using backend.Application.Benefits.Commands;
using backend.Domain;
using backend.Application.Benefits.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Application.DTOs;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitController : ControllerBase
    {
        private readonly GetBenefitsQuery _getBenefitsQuery;
        private readonly GetBenefitByIdQuery _getBenefitByIdQuery;
        private readonly CreateBenefitCommand _createBenefitCommand;
        private readonly DeleteBenefitCommand _deleteBenefitCommand;
        private readonly AssignBenefitsToEmployeeCommand _assignBenefitsCommand;
        private readonly GetAssignedBenefitsQuery _getAssignedBenefitsQuery;
        private readonly UpdateBenefitCommand _updateBenefitCommand;
        private readonly IsBenefitAssignedQuery _isBenefitAssignedQuery;

        public BenefitController(
            GetBenefitsQuery getBenefitsQuery,
            GetBenefitByIdQuery getBenefitByIdQuery,
            CreateBenefitCommand createBenefitCommand,
            DeleteBenefitCommand deleteBenefitCommand,
            AssignBenefitsToEmployeeCommand assignBenefitsCommand,
            GetAssignedBenefitsQuery getAssignedBenefitsQuery,
            UpdateBenefitCommand updateBenefitCommand,
            IsBenefitAssignedQuery isBenefitAssignedQuery)
        {
            _getBenefitsQuery = getBenefitsQuery;
            _getBenefitByIdQuery = getBenefitByIdQuery;
            _createBenefitCommand = createBenefitCommand;
            _deleteBenefitCommand = deleteBenefitCommand;
            _assignBenefitsCommand = assignBenefitsCommand;
            _getAssignedBenefitsQuery = getAssignedBenefitsQuery;
            _updateBenefitCommand = updateBenefitCommand;
            _isBenefitAssignedQuery = isBenefitAssignedQuery;
        }

        [HttpGet("{id}")]
        public ActionResult<Benefit> GetById(Guid id, [FromQuery] string companyId)
        {
            var benefit = _getBenefitByIdQuery.Execute(id, companyId);
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

                var result = _createBenefitCommand.Execute(benefit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error creating benefit: {ex.Message}");
            }
        }

        [HttpDelete]
        public ActionResult<DeleteBenefitDto> DeleteBenefit(Guid benefitId)
        {
            try
            {
                if (benefitId == Guid.Empty)
                    return BadRequest("The benefit id is required.");

                var result = _deleteBenefitCommand.Execute(benefitId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting benefit: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<List<Benefit>> Get([FromQuery] string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return BadRequest("Se requiere el ID de la empresa.");

            var benefits = _getBenefitsQuery.Execute(companyId);
            return Ok(benefits);
        }

        [HttpPost("assign")]
        public ActionResult<bool> AssignBenefitsToEmployee([FromBody] EmployeeBenefitSelection selection)
        {
            try
            {
                if (selection == null || selection.BenefitIds == null || !selection.BenefitIds.Any())
                    return BadRequest("Datos inválidos.");

                var result = _assignBenefitsCommand.Execute(selection.EmployeeId, selection.BenefitIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error assigning benefits: {ex.Message}");
            }
        }

        [HttpGet("assigned")]
        public ActionResult<List<Benefit>> GetAssignedBenefits([FromQuery] Guid employeeId)
        {
            try
            {
                var benefits = _getAssignedBenefitsQuery.Execute(employeeId);
                return Ok(benefits);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error fetching assigned benefits: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<bool> UpdateBenefit(Guid id, [FromBody] Benefit benefit)
        {
            try
            {
                if (benefit == null || id.ToString() != benefit.Id)
                    return BadRequest("ID no válido.");

                var success = _updateBenefitCommand.Execute(benefit);
                if (!success)
                    return Conflict("No se pudo actualizar el beneficio.");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error actualizando beneficio: {ex.Message}");
            }
        }

        [HttpGet("benefits/{benefitId}/is-assigned")]
        public ActionResult<bool> IsBenefitAssigned(Guid benefitId)
        {
            try
            {
                var isAssigned = _isBenefitAssignedQuery.Execute(benefitId);
                return Ok(isAssigned);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error checking if benefit is assigned: {ex.Message}");
            }
        }
    }
}

