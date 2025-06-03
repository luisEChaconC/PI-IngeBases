using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Commands
{
    public class UpdatePayrollIdInTimesheetsCommand : IUpdatePayrollIdInTimesheetsCommand
    {
        private readonly ITimesheetRepository _repository;

        public UpdatePayrollIdInTimesheetsCommand(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Guid newPayrollId, Guid employeeId, DateTime startDate, DateTime endDate)
        {
            var timesheets = _repository.GetTimesheetByEmployeeAndPeriod(employeeId, startDate, endDate);
            foreach (var timesheet in timesheets)
            {
                timesheet.PayrollId = newPayrollId;
            }

            var updated = _repository.UpdatePayrollIdInTimesheets(timesheets);

            if (!updated)
            {
                throw new Exception("Failed to update payrollId in timesheets");
            }

            return updated;
        }
    }
}
