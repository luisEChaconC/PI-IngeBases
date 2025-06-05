using backend.Infraestructure;

namespace backend.Application.Commands
{
    public class ApproveDayCommand : IApproveDayCommand
    {
        private readonly ITimesheetRepository _repository;

        public ApproveDayCommand(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Guid dayId, Guid supervisorId)
        {
            if (dayId == Guid.Empty)
            {
                throw new ArgumentException("DayId cannot be empty", nameof(dayId));
            }

            if (supervisorId == Guid.Empty)
            {
                throw new ArgumentException("SupervisorId cannot be empty", nameof(supervisorId));
            }

            var day = _repository.GetDayById(dayId);
            if (day == null)
            {
                throw new ArgumentException("Day not found", nameof(dayId));
            }

            if (!day.IsSubmitted)
            {
                throw new InvalidOperationException("Day must be submitted before it can be approved");
            }

            if (day.IsApproved)
            {
                throw new InvalidOperationException("Day is already approved");
            }

            return _repository.ApproveDayById(dayId, supervisorId);
        }
    }
} 