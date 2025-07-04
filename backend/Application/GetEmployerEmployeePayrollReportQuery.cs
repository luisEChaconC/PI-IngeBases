using backend.Application.DTOs;
using backend.Infraestructure;

namespace backend.Application.Queries.EmployerPayrollReport
{
    public interface IGetEmployerEmployeePayrollReportQuery
    {
        Task<EmployerEmployeePayrollReportDto> ExecuteAsync(Guid companyId, Guid employerId);
    }

    public class GetEmployerEmployeePayrollReportQuery : IGetEmployerEmployeePayrollReportQuery
    {
        private readonly IEmployerPayrollReportRepository _repository;

        public GetEmployerEmployeePayrollReportQuery(IEmployerPayrollReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmployerEmployeePayrollReportDto> ExecuteAsync(Guid companyId, Guid employerId)
        {
            return await _repository.GetEmployeePayrollReport(companyId, employerId);
        }
    }
}
