using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries
{
    public class GetPendingDaysByEmployeeQuery : IGetPendingDaysByEmployeeQuery
    {
        private readonly ITimesheetRepository _repository;

        public GetPendingDaysByEmployeeQuery(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public List<DayModel> Execute(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                throw new ArgumentException("EmployeeId cannot be empty", nameof(employeeId));
            }

            return _repository.GetPendingDaysByEmployee(employeeId);
        }
    }
} 