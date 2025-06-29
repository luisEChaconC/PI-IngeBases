using backend.Domain;

namespace backend.Application.Payslip.Services
{
    public interface IBuildPayslipItems
    {
        List<PayslipItem> Build(List<DeductionDetailModel> rawDeductions);
    }
}
