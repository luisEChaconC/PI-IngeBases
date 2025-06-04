using backend.Domain;

namespace backend.Infraestructure
{
    public interface ITimesheetRepository
    {
        List<DayModel> GetDaysByTimesheetId(Guid id);
        List<TimesheetModel> GetTimesheetByEmployeeAndPeriod(Guid employeeId, DateTime startDate, DateTime endDate);
        int GetTotalWorkHoursByTimesheetId(Guid timesheetId);
        TimesheetModel? GetTimesheetByEmployeeAndDate(Guid employeeId, DateTime date);
        DayModel? GetDayById(Guid dayId);
        bool UpdateDayWorkDetails(Guid dayId, int hoursWorked, string? description);
        bool UpdatePayrollIdInTimesheets(List<TimesheetModel> timesheets);
        void InsertTimesheetsForPeriod(DateTime periodStartDate, DateTime periodEndDate, Guid employeeId, Guid? payrollId = null);
    }
}