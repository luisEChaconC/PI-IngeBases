using backend.Domain;

namespace backend.Infraestructure
{
    public interface ITimesheetRepository
    {
        List<DayModel> GetDaysByTimesheetId(Guid id);
    }
}