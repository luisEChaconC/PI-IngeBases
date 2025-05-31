using backend.Domain;
using System.Collections.Generic;

namespace backend.Application
{
    public interface IPayrollService
    {
        List<PayrollModel> GetPayrollsByCompanyId(string companyId);
    }
}
