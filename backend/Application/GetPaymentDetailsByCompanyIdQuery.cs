using backend.Domain;
using backend.Infrastructure;

namespace backend.Application.Queries.PaymentDetails
{
    public interface IGetPaymentDetailsByCompanyIdQuery
    {
        Task<List<PaymentDetailModel>> ExecuteAsync(Guid companyId);
    }

    public class GetPaymentDetailsByCompanyIdQuery : IGetPaymentDetailsByCompanyIdQuery
    {
        private readonly IPaymentDetailRepository _repository;

        public GetPaymentDetailsByCompanyIdQuery(IPaymentDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PaymentDetailModel>> ExecuteAsync(Guid companyId)
        {
            return await _repository.GetByCompanyIdAsync(companyId);
        }
    }
}
