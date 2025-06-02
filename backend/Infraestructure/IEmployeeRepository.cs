using backend.Domain;

namespace backend.Infraestructure
{
    public interface IEmployeeRepository
    {
        void CreateEmployee(EmployeeModel employee);
        List<dynamic> GetEmployeesByCompanyId(string companyId);
        Task<List<EmployeeModel>> GetSummaryByCompanyIdAsync(Guid companyId);
    }
}