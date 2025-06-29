using NUnit.Framework;
using Moq;
using backend.Application.Benefits.Commands;
using backend.Application.DTOs;
using backend.Infraestructure;
using System;

namespace backend.Application.Tests.Commands.Benefit
{
    [TestFixture]
    public class DeleteBenefitCommandTest
    {
        [Test]
        public void Execute_CallsRepositoryDeleteBenefit_WithCorrectId()
        {
            var mockRepo = new Mock<IBenefitRepository>();
            var command = new DeleteBenefitCommand(mockRepo.Object);
            var benefitId = Guid.NewGuid();
            var expectedDto = new DeleteBenefitDto { ResultCode = "Deleted", ResultMessage = "Benefit deleted." };

            mockRepo.Setup(r => r.DeleteBenefit(benefitId)).Returns(expectedDto);

            var result = command.Execute(benefitId);

            mockRepo.Verify(r => r.DeleteBenefit(benefitId), Times.Once);
            Assert.AreEqual(expectedDto, result);
        }

        [Test]
        public void Execute_ReturnsNull_WhenRepositoryReturnsNull()
        {
            var mockRepo = new Mock<IBenefitRepository>();
            var command = new DeleteBenefitCommand(mockRepo.Object);
            var benefitId = Guid.NewGuid();

            mockRepo.Setup(r => r.DeleteBenefit(benefitId)).Returns((DeleteBenefitDto)null);

            var result = command.Execute(benefitId);

            Assert.IsNull(result);
        }

        [Test]
        public void Execute_PropagatesException_FromRepository()
        {
            var mockRepo = new Mock<IBenefitRepository>();
            var benefitId = Guid.NewGuid();
            mockRepo.Setup(r => r.DeleteBenefit(benefitId)).Throws(new InvalidOperationException());

            var command = new DeleteBenefitCommand(mockRepo.Object);

            Assert.Throws<InvalidOperationException>(() => command.Execute(benefitId));
        }

        [Test]
        public void Execute_ReturnsExpectedDto_ForDifferentResultCodes()
        {
            var mockRepo = new Mock<IBenefitRepository>();
            var command = new DeleteBenefitCommand(mockRepo.Object);
            var benefitId = Guid.NewGuid();

            var expectedDto = new DeleteBenefitDto { ResultCode = "MarkedAsDeleted", ResultMessage = "Soft deleted." };
            mockRepo.Setup(r => r.DeleteBenefit(benefitId)).Returns(expectedDto);

            var result = command.Execute(benefitId);

            Assert.AreEqual("MarkedAsDeleted", result.ResultCode);
            Assert.AreEqual("Soft deleted.", result.ResultMessage);
        }
    }
}