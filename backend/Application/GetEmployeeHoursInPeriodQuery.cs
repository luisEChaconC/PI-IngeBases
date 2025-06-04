using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries
{
    public class GetEmployeeHoursInPeriodQuery : IGetEmployeeHoursInPeriodQuery
    {
        private readonly ITimesheetRepository _repository;

        public GetEmployeeHoursInPeriodQuery(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public int Execute(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            int totalWorkHours = 0;
            var timesheets = _repository.GetTimesheetByEmployeeAndPeriod(employeeId, startDate, endDate);

            foreach (var timesheet in timesheets) 
            {
                totalWorkHours += _repository.GetTotalWorkHoursByTimesheetId(timesheet.Id);
            }

            return totalWorkHours;
        }
    }
}