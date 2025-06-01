using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries.Payroll
{
    public interface IGetPayrollsByCompanyIdQuery
    {
        Task<List<PayrollModel>> ExecuteAsync(Guid companyId);
    }

    public class GetPayrollsByCompanyIdQuery : IGetPayrollsByCompanyIdQuery
    {
        private readonly IPayrollRepository _repository;

        public GetPayrollsByCompanyIdQuery(IPayrollRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PayrollModel>> ExecuteAsync(Guid companyId)
        {
            return await _repository.GetByCompanyIdAsync(companyId);
        }
    }
}