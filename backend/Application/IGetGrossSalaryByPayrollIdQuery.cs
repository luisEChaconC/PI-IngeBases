public interface IGetGrossSalaryByPayrollIdQuery
{
    decimal Execute(Guid payrollId);
}