using backend.Domain;

namespace backend.Infraestructure
{
    public interface IPayslipRepository
    {
        Task<List<PayslipModel>> GetByEmployeeIdAsync(Guid employeeId);
        Task<PayslipModel?> GetByEmployeeIdAndStartDateAsync(Guid employeeId, DateTime startDate);

        Task<List<DeductionDetailModel>> GetDeductionDetailsAsync(Guid paymentDetailsId);
    }
}
