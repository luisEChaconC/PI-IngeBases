using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Commands.PaymentDetails
{
    public interface ICreatePaymentDetailCommand
    {
        Task<Guid> ExecuteAsync(PaymentDetailModel model);
    }

    public class CreatePaymentDetailCommand : ICreatePaymentDetailCommand
    {
        private readonly IPaymentDetailRepository _repository;

        public CreatePaymentDetailCommand(IPaymentDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> ExecuteAsync(PaymentDetailModel model)
        {
            return await _repository.CreateAsync(model);
        }
    }
}
