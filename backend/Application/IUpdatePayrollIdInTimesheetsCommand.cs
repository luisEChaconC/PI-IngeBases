namespace backend.Application.Commands
{
    public interface IUpdatePayrollIdInTimesheetsCommand
    {
        bool Execute(Guid newPayrollId, Guid employeeId, DateTime startDate, DateTime endDate);
    }
}
