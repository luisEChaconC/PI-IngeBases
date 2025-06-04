namespace backend.Application.Benefits.Queries
{
    using backend.Domain;
    using backend.Infraestructure;

    public class GetBenefitByIdQuery
    {
        private readonly IBenefitRepository _repository;

        public GetBenefitByIdQuery(IBenefitRepository repository)
        {
            _repository = repository;
        }

        public Benefit? Execute(Guid id, string companyId)
        {
            var allBenefits = _repository.GetBenefits(companyId);
            return allBenefits.FirstOrDefault(b => b.Id == id.ToString());
        }
    }
}
