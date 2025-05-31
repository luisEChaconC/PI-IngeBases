using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries
{
    public class GetDaysByTimesheetIdQuery: IGetDaysByTimesheetIdQuery
    {
        private readonly ITimesheetRepository _repository;

        public GetDaysByTimesheetIdQuery(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public List<DayModel> Execute(Guid timesheetId)
        {
            return _repository.GetDaysByTimesheetId(timesheetId);
        }
    }
}