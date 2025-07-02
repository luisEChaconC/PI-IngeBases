using backend.Domain;
using backend.Application.DTOs;
namespace backend.Infraestructure
{
    public interface IEmployerPayrollReportRepository
    {
        Task<EmployerEmployeePayrollReportDto> GetEmployeePayrollReport(Guid companyId, Guid employerId);
    }
}