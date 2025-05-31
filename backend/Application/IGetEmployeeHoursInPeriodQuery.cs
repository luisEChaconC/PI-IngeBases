namespace backend.Application.Queries
{
    public interface IGetEmployeeHoursInPeriodQuery
    {
        int Execute(Guid employeeId, DateTime startDate, DateTime endDate);
    }
}