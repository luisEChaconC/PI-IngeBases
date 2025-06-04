using backend.Domain;

namespace backend.Application.Queries
{
    public interface IGetEmployeeTimesheetByDateQuery
    {
        TimesheetModel? Execute(Guid employeeId, DateTime date);
    }
} 