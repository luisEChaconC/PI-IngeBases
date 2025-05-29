using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries.PaymentDetails
{
    public interface IGetPaymentDetailsByEmployeeIdQuery
    {
        Task<List<PaymentDetailModel>> ExecuteAsync(Guid employeeId);
    }

    public class GetPaymentDetailsByEmployeeIdQuery : IGetPaymentDetailsByEmployeeIdQuery
    {
        private readonly IPaymentDetailRepository _repository;

        public GetPaymentDetailsByEmployeeIdQuery(IPaymentDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PaymentDetailModel>> ExecuteAsync(Guid employeeId)
        {
            return await _repository.GetByEmployeeIdAsync(employeeId);
        }
    }
}