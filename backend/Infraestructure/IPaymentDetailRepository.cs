using backend.Domain;

namespace backend.Infraestructure
{
    public interface IPaymentDetailRepository
    {
        Task<Guid> CreateAsync(PaymentDetailModel model);
        Task<PaymentDetailModel?> GetByIdAsync(Guid id);
        Task<List<PaymentDetailModel>> GetByEmployeeIdAsync(Guid employeeId);
        Task<List<PaymentDetailModel>> GetByCompanyIdAsync(Guid companyId);
    }
}

