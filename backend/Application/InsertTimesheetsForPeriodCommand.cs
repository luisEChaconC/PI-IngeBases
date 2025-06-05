using backend.Application.Commands;
using backend.Infraestructure;

namespace backend.Application.Commands
{
    public class InsertTimesheetsForPeriodCommand : IInsertTimesheetsForPeriodCommand
    {
        private readonly ITimesheetRepository _repository;

        public InsertTimesheetsForPeriodCommand(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public void Execute(DateTime periodStartDate, DateTime periodEndDate, Guid employeeId, Guid? payrollId = null)
        {
            _repository.InsertTimesheetsForPeriod(periodStartDate, periodEndDate, employeeId, payrollId);
        }
    }
} 