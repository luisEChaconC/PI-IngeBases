using backend.Domain;
using System.Collections.Generic;

namespace backend.Infraestructure
{
    public interface IPayrollRepository
    {
        List<PayrollModel> GetPayrollsByCompanyId(string companyId);
    }
}