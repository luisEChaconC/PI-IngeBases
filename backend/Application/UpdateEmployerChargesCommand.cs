using System;
using System.Threading.Tasks;
using backend.Infraestructure;
using backend.Application; 

namespace backend.Application.Commands
{
    public class UpdateEmployerChargesCommand : IUpdateEmployerChargesCommand
    {
        private readonly IPaymentDetailRepository _paymentDetailRepository;

        public UpdateEmployerChargesCommand(IPaymentDetailRepository paymentDetailRepository)
        {
            _paymentDetailRepository = paymentDetailRepository;
        }

        public Task ExecuteAsync(Guid paymentDetailsId, decimal employerCharges)
            => _paymentDetailRepository.UpdateEmployerChargesAsync(paymentDetailsId, employerCharges);
    }
}
