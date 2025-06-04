namespace backend.Application.Commands
{
    public interface IInsertTimesheetsForPeriodCommand
    {
        void Execute(DateTime periodStartDate, DateTime periodEndDate, Guid employeeId, Guid? payrollId = null);
    }
} 