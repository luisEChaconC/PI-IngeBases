using backend.Application.Payslip.Services;
using backend.Domain;
using backend.Infraestructure;

public class GetPayslipByEmployeeIdAndStartDateQuery
{
    private readonly IPayslipRepository _repository;
    private readonly IBuildPayslipItems _builder;

    public GetPayslipByEmployeeIdAndStartDateQuery(IPayslipRepository repository, IBuildPayslipItems builder)
    {
        _repository = repository;
        _builder = builder;
    }

    public async Task<PayslipModel?> ExecuteAsync(Guid employeeId, DateTime startDate)
    {
        var payslip = await _repository.GetByEmployeeIdAndStartDateAsync(employeeId, startDate);
        if (payslip is null) return null;

        var rawItems = await _repository.GetDeductionDetailsAsync(Guid.Parse(payslip.Id!));
        payslip.Items = _builder.Build(rawItems);

        return payslip;
    }
}
