namespace backend.Application.Benefits.Commands
{
    using backend.Infraestructure;

    public class DeleteBenefitCommand
    {
        private readonly IBenefitRepository _repository;

        public DeleteBenefitCommand(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Guid benefitId)
        {
            return _repository.DeleteBenefit(benefitId);
        }
    }
}