using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries.Payroll
{
    public interface ICheckPayrollExistsQuery
    {
        Task<bool> ExecuteAsync(DateTime startDate, DateTime endDate, Guid companyId);
    }

    public class CheckPayrollExistsQuery : ICheckPayrollExistsQuery
    {
        private readonly IPayrollRepository _repository;

        public CheckPayrollExistsQuery(IPayrollRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(DateTime startDate, DateTime endDate, Guid companyId)
        {
            return await _repository.CheckPayrollExistsAsync(companyId, startDate, endDate);
        }
    }
}