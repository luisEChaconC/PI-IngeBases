using backend.Domain;
using backend.Infraestructure;
using backend.Application.Payslip.Services;

namespace backend.Application.Payslip.Queries
{
    public class GetPayslipsByEmployeeIdQuery
    {
        private readonly IPayslipRepository _repository;
        private readonly IBuildPayslipItems _itemBuilder;

        public GetPayslipsByEmployeeIdQuery(IPayslipRepository repository, IBuildPayslipItems itemBuilder)
        {
            _repository = repository;
            _itemBuilder = itemBuilder;
        }

        public async Task<List<PayslipModel>> ExecuteAsync(Guid employeeId)
        {
            var payslips = await _repository.GetByEmployeeIdAsync(employeeId);

            foreach (var payslip in payslips)
            {
                if (payslip.Id is null)
                    continue;

                var rawItems = await _repository.GetDeductionDetailsAsync(Guid.Parse(payslip.Id));
                payslip.Items = _itemBuilder.Build(rawItems);
            }

            return payslips;
        }
    }
}
