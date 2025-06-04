using backend.Infraestructure;

namespace backend.Application.Benefits.Queries
{
    public class IsBenefitAssignedQuery
    {
        private readonly IBenefitRepository _repository;

        public IsBenefitAssignedQuery(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public bool Execute(Guid benefitId)
        {
            return _repository.IsBenefitAssignedToAnyEmployee(benefitId);
        }
    }
}