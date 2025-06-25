using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Payslip.Queries
{
    public class GetPayslipsByEmployeeIdQuery
    {
        private readonly IPayslipRepository _repository;

        public GetPayslipsByEmployeeIdQuery(IPayslipRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PayslipModel>> ExecuteAsync(Guid employeeId)
        {
            return await _repository.GetByEmployeeIdAsync(employeeId);
        }
    }
}
