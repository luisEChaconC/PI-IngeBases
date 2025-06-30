using System;

namespace backend.Application.Commands.Payroll
{
    public interface IEmployerCostFromPayrollCommand
    {
        void Execute(Guid payrollId);
    }
}
