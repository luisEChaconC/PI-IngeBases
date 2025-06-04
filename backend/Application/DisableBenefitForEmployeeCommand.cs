using backend.Infraestructure;

namespace backend.Application.Commands
{
    public interface IDisableBenefitForEmployeeCommand
    {
        bool Execute(Guid benefitId, Guid employeeId);
    }

    public class DisableBenefitForEmployeeCommand : IDisableBenefitForEmployeeCommand
    {
        private readonly IBenefitRepository _repository;

        public DisableBenefitForEmployeeCommand(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Guid benefitId, Guid employeeId)
        {
            return _repository.DisableBenefitForEmployee(benefitId, employeeId);
        }
    }
}