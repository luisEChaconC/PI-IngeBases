using backend.Domain;

namespace backend.Application
{
    public interface IDeductionDetailRepository
    {
        void InsertDeductionDetail(DeductionDetailModel detail);
    }
}
