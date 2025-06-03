using backend.Application;
using backend.Application.DTOs;
using backend.Domain;
using backend.Application.Queries;
using backend.Application.Commands;
using backend.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimesheetController : ControllerBase
    {
        private readonly IGetDaysByTimesheetIdQuery _getDaysByTimesheetIdQuery;
        private readonly IGetEmployeeHoursInPeriodQuery _getEmployeeHoursInPeriodQuery;
        private readonly IGetEmployeeTimesheetByDateQuery _getEmployeeTimesheetByDateQuery;
        private readonly IUpdateDayCommand _updateDayCommand;
        private readonly IUpdatePayrollIdInTimesheetsCommand _updatePayrollIdInTimesheetsCommand;

        public TimesheetController(IGetDaysByTimesheetIdQuery getDaysByTimesheetIdQuery, IGetEmployeeHoursInPeriodQuery getEmployeeHoursInPeriodQuery, IUpdatePayrollIdInTimesheetsCommand updatePayrollIdInTimesheetsCommand, IGetEmployeeTimesheetByDateQuery getEmployeeTimesheetByDateQuery, IUpdateDayCommand updateDayCommand)
        {
            _getDaysByTimesheetIdQuery = getDaysByTimesheetIdQuery;
            _getEmployeeHoursInPeriodQuery = getEmployeeHoursInPeriodQuery;
            _getEmployeeTimesheetByDateQuery = getEmployeeTimesheetByDateQuery;
            _updateDayCommand = updateDayCommand;
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

        [HttpGet("employee/{employeeId}/timesheet-by-date")]
        public IActionResult GetEmployeeTimesheetByDate(Guid employeeId, [FromQuery] DateTime date)
        {
            try
            {
                if (employeeId == Guid.Empty)
                {
                    return BadRequest("EmployeeId is required.");
                }

                if (date == default || date == DateTime.MinValue)
                {
                    return BadRequest("Date is required and must be a valid date.");
                }

                var timesheet = _getEmployeeTimesheetByDateQuery.Execute(employeeId, date);

                if (timesheet == null)
                {
                    return NotFound("No timesheet found for the specified employee and date.");
                }

                return Ok(timesheet);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while retrieving the employee timesheet.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("/day/{dayId}")]
        public IActionResult UpdateDay(Guid dayId, DayCommandDto dayCommandDto)
        {
            try
            {
                if (dayCommandDto == null)
                {
                    return BadRequest("Day command is required");
                }

                if (dayId == Guid.Empty)
                {
                    return BadRequest("DayId is required");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var day = _updateDayCommand.Execute(dayId, dayCommandDto);

                return Ok(day);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while updating the day",
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