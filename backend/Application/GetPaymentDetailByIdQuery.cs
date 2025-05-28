using backend.Domain;
using backend.Infrastructure;

namespace backend.Application.Queries.PaymentDetails
{
    public interface IGetPaymentDetailByIdQuery
    {
        Task<PaymentDetailModel?> ExecuteAsync(Guid id);
    }

    public class GetPaymentDetailByIdQuery : IGetPaymentDetailByIdQuery
    {
        private readonly IPaymentDetailRepository _repository;

        public GetPaymentDetailByIdQuery(IPaymentDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaymentDetailModel?> ExecuteAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
