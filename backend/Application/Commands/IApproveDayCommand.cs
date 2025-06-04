namespace backend.Application.Commands
{
    public interface IApproveDayCommand
    {
        bool Execute(Guid dayId, Guid supervisorId);
    }
} 