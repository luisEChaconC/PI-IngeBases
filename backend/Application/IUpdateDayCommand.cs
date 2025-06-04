using backend.Application.DTOs;
using backend.Domain;

namespace backend.Application.Commands
{
    public interface IUpdateDayCommand
    {
        bool Execute(Guid dayId, DayCommandDto dayDto);
    }
}