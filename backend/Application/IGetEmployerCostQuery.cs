public interface IGetEmployerCostQuery
{
    EmployerCostModel? Execute(Guid payrollId);
}