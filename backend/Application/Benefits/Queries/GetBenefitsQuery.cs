namespace backend.Application.Benefits.Queries
{
    using backend.Domain;
    using backend.Infraestructure;

    public class GetBenefitsQuery
    {
        private readonly IBenefitRepository _repository;

        public GetBenefitsQuery(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public List<Benefit> Execute(string companyId)
        {
            return _repository.GetBenefits(companyId);
        }
    }
}

