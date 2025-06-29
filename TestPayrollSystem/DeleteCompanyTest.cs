using NUnit.Framework;
using Moq;
using backend.Application.Commands.Company;
using backend.Infraestructure;
using System;

namespace backend.Application.Tests.Commands.Company
{
    [TestFixture]
    public class DeleteCompanyCommandTest
    {
        [Test]
        public void Execute_CallsRepositoryDeleteCompany_WithCorrectId()
        {
            var mockRepo = new Mock<ICompanyRepository>();
            var command = new DeleteCompanyCommand(mockRepo.Object);
            var companyId = "F7D6626A-B039-45FR-A077-BBBF3A2BA000";

            command.Execute(companyId);

            mockRepo.Verify(r => r.DeleteCompany(companyId), Times.Once);
        }

        [Test]
        public void Execute_ThrowsArgumentNullException_WhenCompanyIdIsNull()
        {
            var mockRepo = new Mock<ICompanyRepository>();
            var command = new DeleteCompanyCommand(mockRepo.Object);

            Assert.Throws<ArgumentNullException>(() => command.Execute(null));
        }

        [Test]
        public void Execute_DoesNotCallRepository_WhenCompanyIdIsEmpty()
        {
            var mockRepo = new Mock<ICompanyRepository>();
            var command = new DeleteCompanyCommand(mockRepo.Object);

            Assert.Throws<ArgumentException>(() => command.Execute(string.Empty));
            mockRepo.Verify(r => r.DeleteCompany(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Execute_PropagatesException_FromRepository()
        {
            var mockRepo = new Mock<ICompanyRepository>();
            var companyId = "F7D6626A-B039-45FR-A077-BBBF3A2BA000";
            mockRepo.Setup(r => r.DeleteCompany(companyId)).Throws(new InvalidOperationException());

            var command = new DeleteCompanyCommand(mockRepo.Object);

            Assert.Throws<InvalidOperationException>(() => command.Execute(companyId));
        }
    }
}