using backend.Domain;

namespace backend.Infraestructure
{
    public interface IPayrollRepository
    {
        Task<List<PayrollModel>> GetByCompanyIdAsync(Guid companyId);
    }
}