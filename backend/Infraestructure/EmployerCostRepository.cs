using backend.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace backend.Infraestructure
{
    public class EmployerCostRepository : IEmployerCostRepository
    {
        private readonly string _connectionString;

        public EmployerCostRepository()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        }

        public void Insert(EmployerCostModel cost)
        {
            var query = @"
                INSERT INTO EmployerCost (
                    Id, PayrollId,
                    SEM, IVM,
                    BPOP_OtherInstitutions, FamilyAllocations, IMAS, INA,
                    BPOP_LPT, FCL, OPC, INS,
                    PrivateInsurance, SolidarityAssociation,
                    LegalDeductionsTotal, BenefitsTotal, TotalEmployerCost
                )
                VALUES (
                    @Id, @PayrollId,
                    @SEM, @IVM,
                    @BPOP_OtherInstitutions, @FamilyAllocations, @IMAS, @INA,
                    @BPOP_LPT, @FCL, @OPC, @INS,
                    @PrivateInsurance, @SolidarityAssociation,
                    @LegalDeductionsTotal, @BenefitsTotal, @TotalEmployerCost
                )";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", cost.Id);
            command.Parameters.AddWithValue("@PayrollId", cost.PayrollId);
            command.Parameters.AddWithValue("@SEM", cost.SEM);
            command.Parameters.AddWithValue("@IVM", cost.IVM);
            command.Parameters.AddWithValue("@BPOP_OtherInstitutions", cost.BPOP_OtherInstitutions);
            command.Parameters.AddWithValue("@FamilyAllocations", cost.FamilyAllocations);
            command.Parameters.AddWithValue("@IMAS", cost.IMAS);
            command.Parameters.AddWithValue("@INA", cost.INA);
            command.Parameters.AddWithValue("@BPOP_LPT", cost.BPOP_LPT);
            command.Parameters.AddWithValue("@FCL", cost.FCL);
            command.Parameters.AddWithValue("@OPC", cost.OPC);
            command.Parameters.AddWithValue("@INS", cost.INS);
            command.Parameters.AddWithValue("@PrivateInsurance", cost.PrivateInsurance);
            command.Parameters.AddWithValue("@SolidarityAssociation", cost.SolidarityAssociation);
            command.Parameters.AddWithValue("@LegalDeductionsTotal", cost.LegalDeductionsTotal);
            command.Parameters.AddWithValue("@BenefitsTotal", cost.BenefitsTotal);
            command.Parameters.AddWithValue("@TotalEmployerCost", cost.TotalEmployerCost);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public EmployerCostModel? GetByPayrollId(Guid payrollId)
        {
            var query = "SELECT * FROM EmployerCost WHERE PayrollId = @PayrollId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PayrollId", payrollId);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return Map(reader);
            }

            return null;
        }

        private EmployerCostModel Map(SqlDataReader reader)
        {
            return new EmployerCostModel
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                PayrollId = reader.GetGuid(reader.GetOrdinal("PayrollId")),
                SEM = reader.GetDecimal(reader.GetOrdinal("SEM")),
                IVM = reader.GetDecimal(reader.GetOrdinal("IVM")),
                BPOP_OtherInstitutions = reader.GetDecimal(reader.GetOrdinal("BPOP_OtherInstitutions")),
                FamilyAllocations = reader.GetDecimal(reader.GetOrdinal("FamilyAllocations")),
                IMAS = reader.GetDecimal(reader.GetOrdinal("IMAS")),
                INA = reader.GetDecimal(reader.GetOrdinal("INA")),
                BPOP_LPT = reader.GetDecimal(reader.GetOrdinal("BPOP_LPT")),
                FCL = reader.GetDecimal(reader.GetOrdinal("FCL")),
                OPC = reader.GetDecimal(reader.GetOrdinal("OPC")),
                INS = reader.GetDecimal(reader.GetOrdinal("INS")),
                PrivateInsurance = reader.GetDecimal(reader.GetOrdinal("PrivateInsurance")),
                SolidarityAssociation = reader.GetDecimal(reader.GetOrdinal("SolidarityAssociation")),
                LegalDeductionsTotal = reader.GetDecimal(reader.GetOrdinal("LegalDeductionsTotal")),
                BenefitsTotal = reader.GetDecimal(reader.GetOrdinal("BenefitsTotal")),
                TotalEmployerCost = reader.GetDecimal(reader.GetOrdinal("TotalEmployerCost"))
            };
        }
    }
}
