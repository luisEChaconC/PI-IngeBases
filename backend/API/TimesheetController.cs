using backend.Application;
using backend.Application.DTOs;
using backend.Domain;
using backend.Application.Queries;
using backend.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetController : ControllerBase
    {
        private readonly IGetDaysByTimesheetIdQuery _getDaysByTimesheetIdQuery;
        private readonly IGetEmployeeHoursInPeriodQuery _getEmployeeHoursInPeriodQuery;
        private readonly IUpdatePayrollIdInTimesheetsCommand _updatePayrollIdInTimesheetsCommand;

        public TimesheetController(IGetDaysByTimesheetIdQuery getDaysByTimesheetIdQuery, IGetEmployeeHoursInPeriodQuery getEmployeeHoursInPeriodQuery, IUpdatePayrollIdInTimesheetsCommand updatePayrollIdInTimesheetsCommand)
        {
            _getDaysByTimesheetIdQuery = getDaysByTimesheetIdQuery;
            _getEmployeeHoursInPeriodQuery = getEmployeeHoursInPeriodQuery;
            _updatePayrollIdInTimesheetsCommand = updatePayrollIdInTimesheetsCommand;
        }

        [HttpGet("{timesheetId}/days")]
        public IActionResult GetDaysByTimesheetId(Guid timesheetId)
        {
            try
            {
                if (timesheetId == Guid.Empty)
                {
                    return BadRequest("TimesheetId is required");
                }

                var days = _getDaysByTimesheetIdQuery.Execute(timesheetId);
                return Ok(days);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error ocurred while retrieving the timesheet days",
                    error = ex.Message
                });
            }
        }

        [HttpGet("employee/{employeeId}/hours")]
        public IActionResult GetEmployeeHoursInPeriod(Guid employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (employeeId == Guid.Empty)
                {
                    return BadRequest("EmployeeId is required");
                }

                if (startDate == default || endDate == default)
                {
                    return BadRequest("StartDate and EndDate are required");
                }

                if (endDate <= startDate)
                {
                    return BadRequest("EndDate must be after StartDate");
                }

                int hours = _getEmployeeHoursInPeriodQuery.Execute(employeeId, startDate, endDate);

                return Ok(new { TotalHours = hours });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving employee hours",
                    error = ex.Message
                });
            }
        }

        [HttpPut("employee/{employeeId}/payroll-id")]
        public IActionResult UpdatePayrollIdInTimesheets(Guid employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] Guid newPayrollId)
        {
            try
            {
                if (employeeId == Guid.Empty)
                {
                    return BadRequest("EmployeeId is required");
                }

                if (startDate == default)
                {
                    return BadRequest("StartDate is required");
                }

                if (endDate == default)
                {
                    return BadRequest("EndDate is required");
                }

                if (newPayrollId == Guid.Empty)
                {
                    return BadRequest("NewPayrollId is required");
                }

                if (endDate <= startDate)
                {
                    return BadRequest("EndDate must be after StartDate");
                }

                var result = _updatePayrollIdInTimesheetsCommand.Execute(newPayrollId, employeeId, startDate, endDate);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while updating payrollId in timesheets",
                    error = ex.Message
                });
            }
        }
    }
}