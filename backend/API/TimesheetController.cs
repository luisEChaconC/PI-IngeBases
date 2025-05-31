using backend.Application;
using backend.Application.DTOs;
using backend.Domain;
using backend.Application.Queries;
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

        public TimesheetController(IGetDaysByTimesheetIdQuery getDaysByTimesheetIdQuery, IGetEmployeeHoursInPeriodQuery getEmployeeHoursInPeriodQuery)
        {
            _getDaysByTimesheetIdQuery = getDaysByTimesheetIdQuery;
            _getEmployeeHoursInPeriodQuery = getEmployeeHoursInPeriodQuery;
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
        public IActionResult GetEmployeeHoursInPeriod(Guid employeeId, [FromQuery] EmployeeHoursPeriodDto request)
        {
            try
            {
                if (employeeId == Guid.Empty)
                {
                    return BadRequest("EmployeeId is required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int hours = _getEmployeeHoursInPeriodQuery.Execute(employeeId, request.StartDate, request.EndDate);

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
    }
}