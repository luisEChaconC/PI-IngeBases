using backend.Application.Commands;
using backend.Application.DTOs;
using backend.Domain;
using backend.Infraestructure;

namespace backend.Application
{
    public class UpdateDayCommand : IUpdateDayCommand
    {
        private readonly ITimesheetRepository _repository;

        public UpdateDayCommand(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Guid dayId, DayCommandDto dayDto)
        {
            
            if (dayDto == null)
            {
                throw new ArgumentNullException(nameof(dayDto));
            }

            if (dayId == Guid.Empty)
            {
                throw new ArgumentException("Day ID is required", nameof(dayId));
            }

            var day = _repository.GetDayById(dayId);

            if (day == null)
            {
                throw new ArgumentException("Day not found");
            }

            if (day.IsApproved)
            {
                throw new InvalidOperationException("Day is already approved");
            }

            var updated = _repository.UpdateDayWorkDetails(dayId, dayDto.WorkedHours, dayDto.Description);
            
            if (!updated)
                throw new ArgumentException("Day not found or could not be updated");
            
            return updated;
        }
    }
}
