namespace backend.Application.Benefits.Commands
{
    using backend.Domain;
    using backend.Infraestructure;

    public class UpdateBenefitCommand
    {
        private readonly IBenefitRepository _repository;

        public UpdateBenefitCommand(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Benefit benefit)
        {
            return _repository.UpdateBenefit(benefit);
        }
    }
}
