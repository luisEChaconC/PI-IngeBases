using backend.Domain;

namespace backend.Application.Queries
{
    public interface IGetPendingApprovalsByEmployeeQuery
    {
        List<PendingApprovalSummary> Execute();
        List<PendingApprovalWithEmployeeInfo> ExecuteWithEmployeeInfo(Guid companyId);
    }
} 