using backend.Domain;
using backend.Application.DTOs;
namespace backend.Infraestructure
{
    public interface IPayrollRepository
    {
        Task<Guid> CreateAsync(PayrollModel model);
        Task<List<PayrollModel>> GetByCompanyIdAsync(Guid companyId);
        Task<List<PayrollSummaryDto>> GetSummaryByCompanyIdAsync(Guid companyId);
        Task<bool> CheckPayrollExistsAsync(Guid companyId, DateTime startDate, DateTime endDate);

    }
}