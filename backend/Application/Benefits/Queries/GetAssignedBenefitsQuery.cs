using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Benefits.Queries
{
    public class GetAssignedBenefitsQuery
    {
        private readonly IBenefitRepository _repository;

        public GetAssignedBenefitsQuery(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public List<Benefit> Execute(Guid employeeId)
        {
            return _repository.GetAssignedBenefitsForEmployee(employeeId);
        }
    }
}

