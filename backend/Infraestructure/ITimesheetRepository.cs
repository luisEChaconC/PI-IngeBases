using backend.Domain;

namespace backend.Infraestructure
{
    public interface ITimesheetRepository
    {
        List<DayModel> GetDaysByTimesheetId(Guid id);
        List<TimesheetModel> GetTimesheetByEmployeeAndPeriod(Guid employeeId, DateTime startDate, DateTime endDate);
        int GetTotalWorkHoursByTimesheetId(Guid timesheetId);
    }
}