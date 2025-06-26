using backend.Application.DTOs;
using backend.Domain;

namespace backend.Repositories
{
    public interface ICompanyReportRepository
    {
        Task<List<CompanyReportDto>> GetCompanyReportsAsync(DateTime startDate, DateTime endDate);
    }
}