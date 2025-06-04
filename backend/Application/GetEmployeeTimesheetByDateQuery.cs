using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries
{
    public class GetEmployeeTimesheetByDateQuery : IGetEmployeeTimesheetByDateQuery
    {
        private readonly ITimesheetRepository _repository;

        public GetEmployeeTimesheetByDateQuery(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public TimesheetModel? Execute(Guid employeeId, DateTime date)
        {
            return _repository.GetTimesheetByEmployeeAndDate(employeeId, date);
        }
    }
} 