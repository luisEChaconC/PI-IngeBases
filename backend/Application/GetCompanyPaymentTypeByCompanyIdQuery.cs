using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries.Company
{
    public interface IGetCompanyPaymentTypeByCompanyIdQuery
    {
        Task<string> ExecuteAsync(Guid companyId);
    }

    public class GetCompanyPaymentTypeByCompanyIdQuery : IGetCompanyPaymentTypeByCompanyIdQuery
    {
        private readonly ICompanyRepository _repository;

        public GetCompanyPaymentTypeByCompanyIdQuery(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> ExecuteAsync(Guid companyId)
        {
            return await _repository.GetPaymentTypeByIdAsync(companyId);
        }
    }
}

