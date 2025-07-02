using NUnit.Framework;
using Moq;
using backend.Application.DTOs;
using backend.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Tests.Queries.Company
{
    [TestFixture]
    public class GetCompanyReportsTests
    {
        [Test]
        public async Task ExecuteAsync_CallsRepositoryWithCorrectDates_AndReturnsResult()
        {
            var mockRepo = new Mock<ICompanyReportRepository>();
            var expected = new List<CompanyReportDto> { new CompanyReportDto() };
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 31);

            mockRepo.Setup(r => r.GetCompanyReportsAsync(start, end)).ReturnsAsync(expected);

            var query = new GetCompanyReportsQuery(mockRepo.Object);

            var result = await query.ExecuteAsync(start, end);

            Assert.That(result, Is.EqualTo(expected));
            mockRepo.Verify(r => r.GetCompanyReportsAsync(start, end), Times.Once);
        }

        [Test]
        public async Task ExecuteAllAsync_CallsRepositoryAndReturnsResult()
        {
            var mockRepo = new Mock<ICompanyReportRepository>();
            var expected = new List<CompanyReportDto> { new CompanyReportDto() };

            mockRepo.Setup(r => r.GetAllCompanyReportsAsync()).ReturnsAsync(expected);

            var query = new GetCompanyReportsQuery(mockRepo.Object);

            var result = await query.ExecuteAllAsync();

            Assert.That(result, Is.EqualTo(expected));
            mockRepo.Verify(r => r.GetAllCompanyReportsAsync(), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WhenNoReports_ReturnsEmptyList()
        {
            var mockRepo = new Mock<ICompanyReportRepository>();
            var start = new DateTime(2025, 2, 1);
            var end = new DateTime(2025, 2, 28);

            mockRepo.Setup(r => r.GetCompanyReportsAsync(start, end))
                    .ReturnsAsync(new List<CompanyReportDto>());

            var query = new GetCompanyReportsQuery(mockRepo.Object);

            var result = await query.ExecuteAsync(start, end);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ExecuteAsync_WhenStartDateIsAfterEndDate_ThrowsException()
        {
            var mockRepo = new Mock<ICompanyReportRepository>();
            var start = new DateTime(2025, 3, 1);
            var end = new DateTime(2025, 2, 1);

            var query = new GetCompanyReportsQuery(mockRepo.Object);

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await query.ExecuteAsync(start, end);
            });
        }

    }
}