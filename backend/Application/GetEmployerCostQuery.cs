public class GetEmployerCostQuery : IGetEmployerCostQuery
{
    private readonly IEmployerCostRepository _repository;

    public GetEmployerCostQuery(IEmployerCostRepository repository)
    {
        _repository = repository;
    }

    public EmployerCostModel? Execute(Guid payrollId)
    {
        return _repository.GetByPayrollId(payrollId);
    }
}
