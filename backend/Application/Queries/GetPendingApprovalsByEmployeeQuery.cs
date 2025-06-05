using backend.Domain;
using backend.Infraestructure;

namespace backend.Application.Queries
{
    public class GetPendingApprovalsByEmployeeQuery : IGetPendingApprovalsByEmployeeQuery
    {
        private readonly ITimesheetRepository _repository;

        public GetPendingApprovalsByEmployeeQuery(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        public List<PendingApprovalSummary> Execute()
        {
            return _repository.GetPendingApprovalsByEmployee();
        }

        public List<PendingApprovalWithEmployeeInfo> ExecuteWithEmployeeInfo(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentException("CompanyId cannot be empty", nameof(companyId));
            }

            return _repository.GetPendingApprovalsWithEmployeeInfo(companyId);
        }
    }
} 