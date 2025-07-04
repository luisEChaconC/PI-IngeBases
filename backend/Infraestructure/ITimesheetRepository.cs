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
        List<PendingApprovalSummary> GetPendingApprovalsByEmployee();
        List<PendingApprovalWithEmployeeInfo> GetPendingApprovalsWithEmployeeInfo(Guid companyId);
        List<DayModel> GetPendingDaysByEmployee(Guid employeeId);
        bool ApproveDayById(Guid dayId, Guid supervisorId);
        void InsertTimesheetsForPeriod(DateTime periodStartDate, DateTime periodEndDate, Guid employeeId, Guid? payrollId = null);
    }
}