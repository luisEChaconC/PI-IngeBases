using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries.Employees
{
    public interface IGetEmployeesByCompanyIdQuery
    {
        Task<List<EmployeeModel>> ExecuteAsync(Guid companyId);
    }

    public class GetEmployeesByCompanyIdQuery : IGetEmployeesByCompanyIdQuery
    {
        private readonly IEmployeeRepository _repository;

        public GetEmployeesByCompanyIdQuery(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<EmployeeModel>> ExecuteAsync(Guid companyId)
        {
            return await _repository.GetSummaryByCompanyIdAsync(companyId);
        }
    }
}
