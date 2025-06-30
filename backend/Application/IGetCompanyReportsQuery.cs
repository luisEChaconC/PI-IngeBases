using backend.Application.DTOs;
using backend.Repositories;

public interface IGetCompanyReportsQuery
{
    Task<List<CompanyReportDto>> ExecuteAsync(DateTime startDate, DateTime endDate);
    Task<List<CompanyReportDto>> ExecuteAllAsync();
}

public class GetCompanyReportsQuery : IGetCompanyReportsQuery
{
    private readonly ICompanyReportRepository _repository;
    public GetCompanyReportsQuery(ICompanyReportRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<CompanyReportDto>> ExecuteAsync(DateTime startDate, DateTime endDate)
    {
        return await _repository.GetCompanyReportsAsync(startDate, endDate);
    }

    public async Task<List<CompanyReportDto>> ExecuteAllAsync()
    {
        return await _repository.GetAllCompanyReportsAsync();
    }
}