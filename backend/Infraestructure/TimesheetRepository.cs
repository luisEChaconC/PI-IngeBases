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
                WHERE TimesheetId = @TimesheetId AND HoursWorked IS NOT NULL AND IsApproved = 1";
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

        public TimesheetModel? GetTimesheetByEmployeeAndDate(Guid employeeId, DateTime date)
        {
            TimesheetModel? timesheet = null;
            var query = @"SELECT Id, StartDate, EndDate, PayrollId
                FROM Timesheets
                WHERE EmployeeId = @EmployeeId
                    AND @Date >= StartDate AND @Date <= EndDate";
            try
            {
                _connection.Open();
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@Date", date.Date);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            timesheet = new TimesheetModel
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                EmployeeId = employeeId,
                                PayrollId = reader["PayrollId"] != DBNull.Value ? reader.GetGuid(reader.GetOrdinal("PayrollId")) : (Guid?)null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving timesheet: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return timesheet;
        }

        public DayModel? GetDayById(Guid dayId)
        {
            DayModel? day = null;
            var query = @"SELECT Id, Date, HoursWorked, WorkDescription, IsApproved, TimesheetId, SupervisorId 
                          FROM Days WHERE Id = @DayId";
            
            try
            {
                _connection.Open();
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@DayId", dayId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            day = new DayModel
                            {
                                Id = reader.GetGuid("Id"),
                                Date = reader.GetDateTime("Date"),
                                HoursWorked = reader["HoursWorked"] != DBNull.Value ? reader.GetInt32("HoursWorked") : null,
                                WorkDescription = reader["WorkDescription"] != DBNull.Value ? reader.GetString("WorkDescription") : null,
                                IsApproved = reader.GetBoolean("IsApproved"),
                                TimesheetId = reader.GetGuid("TimesheetId"),
                                SupervisorId = reader["SupervisorId"] != DBNull.Value ? reader.GetGuid("SupervisorId") : null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving day: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return day;
        }

        public bool UpdateDayWorkDetails(Guid dayId, int hoursWorked, string? description)
        {
            var query = @"UPDATE Days 
                          SET HoursWorked = @HoursWorked, 
                              WorkDescription = @WorkDescription
                          WHERE Id = @Id";
            
            try
            {
                _connection.Open();
                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Id", dayId);
                    command.Parameters.AddWithValue("@HoursWorked", hoursWorked);
                    command.Parameters.AddWithValue("@WorkDescription", (object?)description ?? DBNull.Value);
                    
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating day: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public bool UpdatePayrollIdInTimesheets(List<TimesheetModel> timesheets)
        {
            var query = @"UPDATE Timesheets 
                          SET PayrollId = @PayrollId 
                          WHERE Id = @Id";
            
            try
            {
                _connection.Open();
                foreach (var timesheet in timesheets)
                {
                    using (var command = new SqlCommand(query, _connection))
                    {
                        command.Parameters.AddWithValue("@PayrollId", (object?)timesheet.PayrollId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Id", timesheet.Id);
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating payrollId in timesheets: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}