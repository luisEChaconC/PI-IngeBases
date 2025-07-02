using NUnit.Framework;
using Moq;
using backend.Application.Commands.Employee;
using backend.Infraestructure;
using System;

namespace backend.Application.Tests.Commands.Employee
{
    [TestFixture]
    public class DeleteEmployeeCommandTest
    {
        [Test]
        public void Execute_CallsRepositoryDeleteEmployee_WithCorrectId()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var command = new DeleteEmployeeCommand(mockRepo.Object);
            var employeeId = "5F1EBB09-0171-490B-95E3-91F63D54879F";

            command.Execute(employeeId);

            mockRepo.Verify(r => r.DeleteEmployee(employeeId), Times.Once);
        }

        [Test]
        public void Execute_ThrowsArgumentNullException_WhenEmployeeIdIsNull()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var command = new DeleteEmployeeCommand(mockRepo.Object);

            Assert.Throws<ArgumentNullException>(() => command.Execute(null));
        }

        [Test]
        public void Execute_DoesNotCallRepository_WhenEmployeeIdIsEmpty()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var command = new DeleteEmployeeCommand(mockRepo.Object);

            Assert.Throws<ArgumentException>(() => command.Execute(string.Empty));
            mockRepo.Verify(r => r.DeleteEmployee(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Execute_PropagatesException_FromRepository()
        {
            var mockRepo = new Mock<IEmployeeRepository>();
            var employeeId = "F7D6626A-B039-45FR-A077-BBBF3A2BA000";
            mockRepo.Setup(r => r.DeleteEmployee(employeeId)).Throws(new InvalidOperationException());

            var command = new DeleteEmployeeCommand(mockRepo.Object);

            Assert.Throws<InvalidOperationException>(() => command.Execute(employeeId));
        }
    }
}
