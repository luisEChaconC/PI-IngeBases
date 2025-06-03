using backend.Domain;

public interface IInsertDeductionDetailsCommand
{
    void Execute(IEnumerable<DeductionDetailModel> details);
}