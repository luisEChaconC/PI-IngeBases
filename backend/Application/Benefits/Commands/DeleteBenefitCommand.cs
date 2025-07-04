namespace backend.Application.Benefits.Commands
{
    using backend.Application.DTOs;
    using backend.Infraestructure;

    public class DeleteBenefitCommand
    {
        private readonly IBenefitRepository _repository;

        public DeleteBenefitCommand(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public DeleteBenefitDto Execute(Guid benefitId)
        {
            return _repository.DeleteBenefit(benefitId);
        }
    }
}