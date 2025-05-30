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
    }
}