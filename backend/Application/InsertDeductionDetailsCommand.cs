using backend.Application;
using backend.Domain;

public class InsertDeductionDetailsCommand : IInsertDeductionDetailsCommand
{
    private readonly IDeductionDetailRepository _repository;

    public InsertDeductionDetailsCommand(IDeductionDetailRepository repository)
    {
        _repository = repository;
    }

    public void Execute(IEnumerable<DeductionDetailModel> details)
    {
        foreach (var detail in details)
        {
            _repository.InsertDeductionDetail(detail);
        }
    }
}