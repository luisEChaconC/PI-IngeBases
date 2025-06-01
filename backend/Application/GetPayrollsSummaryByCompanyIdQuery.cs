using backend.Application.DTOs;
using backend.Infraestructure;

namespace backend.Application.Queries.Payroll
{
    public interface IGetPayrollsSummaryByCompanyIdQuery
    {
        Task<List<PayrollSummaryDto>> ExecuteAsync(Guid companyId);
    }

    public class GetPayrollsSummaryByCompanyIdQuery : IGetPayrollsSummaryByCompanyIdQuery
    {
        private readonly IPayrollRepository _repository;

        public GetPayrollsSummaryByCompanyIdQuery(IPayrollRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PayrollSummaryDto>> ExecuteAsync(Guid companyId)
        {
            return await _repository.GetSummaryByCompanyIdAsync(companyId);
        }
    }
}