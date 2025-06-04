using backend.Domain;

namespace backend.Application.Queries
{
    public interface IGetDaysByTimesheetIdQuery
    {
        List<DayModel> Execute(Guid timesheetId);
    }
}