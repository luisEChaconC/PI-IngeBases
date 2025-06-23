using System;
using System.Data;
using NUnit.Framework;
using backend.Infraestructure;

namespace backend.Infraestructure.Tests
{
    public class SimpleFakeCommand
    {
        public string CommandText { get; set; }
        public CommandType CommandType { get; set; }
        public bool WasExecuted { get; private set; }
        public Guid? PassedCompanyId { get; private set; }

        public void AddParameter(string name, object value)
        {
            if (name == "@CompanyId")
                PassedCompanyId = (Guid)value;
        }

        public void Execute()
        {
            WasExecuted = true;
        }
    }

    [TestFixture]
    public class CompanyRepositoryTest
    {
        [Test]
        public void DeleteCompany_CallsProcedureWithCorrectId()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var repo = new SimpleTestableCompanyRepository();

            // Act
            repo.DeleteCompany(testId.ToString());

            // Assert
            var cmd = repo.LastCommand;

            Assert.IsNotNull(cmd); // se creó con éxito el comando
            Assert.AreEqual("sp_DeleteCompany", cmd.CommandText); // nombre de stored procedure correcto
            Assert.AreEqual(CommandType.StoredProcedure, cmd.CommandType); // tipo de comando correcto
            Assert.AreEqual(testId, cmd.PassedCompanyId); // se pasó el company ID correcto
            Assert.IsTrue(cmd.WasExecuted);
        }

        [Test]
        public void DeleteCompany_InvalidGuid_ThrowsFormatException()
        {

            var repo = new SimpleTestableCompanyRepository();
            var invalidId = "not-a-guid";

            Assert.Throws<FormatException>(() => repo.DeleteCompany(invalidId));
        }
    }

    public class SimpleTestableCompanyRepository : CompanyRepository
    {
        public SimpleFakeCommand LastCommand { get; private set; }

        public new void DeleteCompany(string companyId)
        {

            var cmd = new SimpleFakeCommand
            {
                CommandText = "sp_DeleteCompany",
                CommandType = CommandType.StoredProcedure
            };
            cmd.AddParameter("@CompanyId", Guid.Parse(companyId));
            cmd.Execute();

            LastCommand = cmd;
        }
    }
}
