using backend.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace backend.Infraestructure
{
    public class TimesheetRepository : BaseRepository, ITimesheetRepository
    {
        private readonly string _connectionString;

        public TimesheetRepository() : base()
        {
        }

        public List<TimesheetModel> GetTimesheetByEmployeeAndPeriod(Guid employeeId, DateTime startDate, DateTime endDate)
        {
            var timesheets = new List<TimesheetModel>();
            var query = @"SELECT Id, StartDate, EndDate, PayrollId
                FROM Timesheets
                WHERE (EmployeeId = @EmployeeId)
                    AND (StartDate >= @PeriodStartDate AND EndDate <= @PeriodEndDate)
                ORDER BY StartDate ASC";
            try
            {
                _connection.Open();
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@PeriodStartDate", startDate);
                    command.Parameters.AddWithValue("@PeriodEndDate", endDate);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            timesheets.Add(new TimesheetModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                EmployeeId = employeeId,
                                PayrollId = reader["PayrollId"] != DBNull.Value ? reader.GetGuid(reader.GetOrdinal("PayrollId")) : (Guid?)null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving timesheets: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return timesheets;
        }

        public List<DayModel> GetDaysByTimesheetId(Guid timesheetId)
        {
            var days = new List<DayModel>();
            var query = @"SELECT Id, Date, HoursWorked, WorkDescription, IsApproved, TimesheetId, SupervisorId FROM Days
                WHERE TimesheetId = @TimesheetId
                ORDER BY Date ASC";
            try
            {
                _connection.Open();
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@TimesheetId", timesheetId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            days.Add(new DayModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                HoursWorked = reader["HoursWorked"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("HoursWorked")) : (int?)null,
                                WorkDescription = reader["WorkDescription"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("WorkDescription")) : null,
                                IsApproved = reader.GetBoolean(reader.GetOrdinal("IsApproved")),
                                TimesheetId = reader.GetGuid(reader.GetOrdinal("TimesheetId")),
                                SupervisorId = reader["SupervisorId"] != DBNull.Value ? reader.GetGuid(reader.GetOrdinal("SupervisorId")) : (Guid?)null
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving payrolls: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return days;
        }

        public int GetTotalWorkHoursByTimesheetId(Guid timesheetId)
        {
            int totalWorkHours = 0;
            var query = @"SELECT ISNULL(SUM(HoursWorked), 0)
                FROM Days
                WHERE TimesheetId = @TimesheetId AND HoursWorked IS NOT NULL";
            try
            {
                _connection.Open();
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@TimesheetId", timesheetId);
                    var result = command.ExecuteScalar();
                    totalWorkHours = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving total work hours: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return totalWorkHours;
        }
    }
}