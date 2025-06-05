namespace backend.Application.Benefits.Commands
{

    using backend.Domain;
    using backend.Infraestructure;

    public class CreateBenefitCommand
    {
        private readonly IBenefitRepository _repository;

        public CreateBenefitCommand(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Benefit benefit)
        {
            return _repository.CreateBenefit(benefit);
        }
    }
}