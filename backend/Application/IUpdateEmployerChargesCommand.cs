using System;
using System.Threading.Tasks;

namespace backend.Application.Commands
{
    public interface IUpdateEmployerChargesCommand
    {
        Task ExecuteAsync(Guid paymentDetailsId, decimal employerCharges);
    }
}
