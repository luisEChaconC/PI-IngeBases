using backend.Domain;
using backend.Application.DTOs;

namespace backend.Infraestructure
{
    public interface IEmployeeRepository
    {
        void CreateEmployee(EmployeeModel employee);
        List<dynamic> GetEmployeesByCompanyId(string companyId);
        Task<List<EmployeeSummaryDto>> GetSummaryByCompanyIdAsync(Guid companyId);
    }
}