using backend.Domain;

namespace backend.Application.Queries
{
    public interface IGetPendingDaysByEmployeeQuery
    {
        List<DayModel> Execute(Guid employeeId);
    }
} 