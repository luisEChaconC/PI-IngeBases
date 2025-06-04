namespace backend.Application.Benefits.Commands
{
    using backend.Infraestructure;

    public class AssignBenefitsToEmployeeCommand
    {
        private readonly IBenefitRepository _repository;

        public AssignBenefitsToEmployeeCommand(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Guid employeeId, List<Guid> benefitIds)
        {
            return _repository.AssignBenefitsToEmployee(employeeId, benefitIds);
        }
    }
}

