using backend.Application.Commands;
using backend.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApprovalController : ControllerBase
    {
        private readonly IGetPendingApprovalsByEmployeeQuery _getPendingApprovalsByEmployeeQuery;
        private readonly IGetPendingDaysByEmployeeQuery _getPendingDaysByEmployeeQuery;
        private readonly IApproveDayCommand _approveDayCommand;

        public ApprovalController(
            IGetPendingApprovalsByEmployeeQuery getPendingApprovalsByEmployeeQuery,
            IGetPendingDaysByEmployeeQuery getPendingDaysByEmployeeQuery,
            IApproveDayCommand approveDayCommand)
        {
            _getPendingApprovalsByEmployeeQuery = getPendingApprovalsByEmployeeQuery;
            _getPendingDaysByEmployeeQuery = getPendingDaysByEmployeeQuery;
            _approveDayCommand = approveDayCommand;
        }

        [HttpGet("pending-by-employee")]
        public IActionResult GetPendingApprovalsByEmployee()
        {
            try
            {
                var pendingApprovals = _getPendingApprovalsByEmployeeQuery.Execute();
                return Ok(pendingApprovals);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving pending approvals",
                    error = ex.Message
                });
            }
        }

        [HttpGet("pending-by-employee-with-info")]
        public IActionResult GetPendingApprovalsByEmployeeWithInfo([FromQuery] Guid companyId)
        {
            try
            {
                if (companyId == Guid.Empty)
                {
                    return BadRequest("CompanyId is required");
                }

                var pendingApprovals = _getPendingApprovalsByEmployeeQuery.ExecuteWithEmployeeInfo(companyId);
                return Ok(pendingApprovals);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving pending approvals with employee info",
                    error = ex.Message
                });
            }
        }

        [HttpGet("employee/{employeeId}/pending-days")]
        public IActionResult GetPendingDaysByEmployee(Guid employeeId)
        {
            try
            {
                if (employeeId == Guid.Empty)
                {
                    return BadRequest("EmployeeId is required");
                }

                var pendingDays = _getPendingDaysByEmployeeQuery.Execute(employeeId);
                return Ok(pendingDays);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving pending days",
                    error = ex.Message
                });
            }
        }

        [HttpPost("day/{dayId}/approve")]
        public IActionResult ApproveDay(Guid dayId, [FromQuery] Guid supervisorId)
        {
            try
            {
                if (dayId == Guid.Empty)
                {
                    return BadRequest("DayId is required");
                }

                if (supervisorId == Guid.Empty)
                {
                    return BadRequest("SupervisorId is required");
                }

                var result = _approveDayCommand.Execute(dayId, supervisorId);
                
                if (!result)
                {
                    return BadRequest("Could not approve day");
                }

                return Ok(new { message = "Day approved successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while approving day",
                    error = ex.Message
                });
            }
        }
    }
} 