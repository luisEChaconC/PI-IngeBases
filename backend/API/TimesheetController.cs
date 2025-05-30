using backend.Application;
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

        public TimesheetController(IGetDaysByTimesheetIdQuery getDaysByTimesheetIdQuery)
        {
            _getDaysByTimesheetIdQuery = getDaysByTimesheetIdQuery;
        }

        [HttpGet("days")]
        public IActionResult GetDaysByTimesheetId([FromQuery] Guid timesheetId)
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
    }
}