using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Payslip.Queries
{
    public class GetPayslipByEmployeeIdAndStartDateQuery
    {
        private readonly IPayslipRepository _repository;

        public GetPayslipByEmployeeIdAndStartDateQuery(IPayslipRepository repository)
        {
            _repository = repository;
        }

        public async Task<PayslipModel?> ExecuteAsync(Guid employeeId, DateTime startDate)
        {
            return await _repository.GetByEmployeeIdAndStartDateAsync(employeeId, startDate);
        }
    }
}
