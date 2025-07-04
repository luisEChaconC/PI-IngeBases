using backend.Domain;

public interface IEmployerCostStrategy
{
    EmployerCostModel Calculate(Guid payrollId, decimal grossSalary);
}