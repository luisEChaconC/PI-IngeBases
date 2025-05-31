using backend.Domain;
using backend.Application.DTOs;
namespace backend.Infraestructure
{
    public interface IPayrollRepository
    {
        Task<List<PayrollModel>> GetByCompanyIdAsync(Guid companyId);
        Task<List<PayrollSummaryDto>> GetSummaryByCompanyIdAsync(Guid companyId);
    }
}