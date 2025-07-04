using backend.Domain;

public interface IEmployerCostRepository
{
    void Insert(EmployerCostModel cost);
    EmployerCostModel? GetByPayrollId(Guid payrollId);
}

