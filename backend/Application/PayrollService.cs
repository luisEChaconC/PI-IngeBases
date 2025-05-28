using backend.Domain;
using backend.Infraestructure;
using System.Collections.Generic;

namespace backend.Application
{
    public class PayrollService : IPayrollService
    {
        private readonly IPayrollRepository _payrollRepository;
        public PayrollService(IPayrollRepository payrollRepository)
        {
            _payrollRepository = payrollRepository;
        }

        public List<PayrollModel> GetPayrollsByCompanyId(string companyId)
        {
            return _payrollRepository.GetPayrollsByCompanyId(companyId);
        }
    }
}
