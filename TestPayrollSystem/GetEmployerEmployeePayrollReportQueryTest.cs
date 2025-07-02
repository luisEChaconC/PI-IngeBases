using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Application.DTOs;
using backend.Application.Queries.EmployerPayrollReport;
using backend.Infraestructure;

namespace backend.Application.Tests.Queries.Payroll
{
    [TestFixture]
    public class GetEmployerEmployeePayrollReportQueryTest
    {
        [Test]
        public async Task ExecuteAsync_ReturnsReport_WhenDataExists()
        {
            var mockRepo = new Mock<IEmployerPayrollReportRepository>();
            var companyId = Guid.NewGuid();
            var employerId = Guid.NewGuid();

            var expectedReport = new EmployerEmployeePayrollReportDto
            {
                CompanyName = "DentalCare",
                EmployerName = "Dr. Smith",
                Employees = new List<EmployeePayrollInfoDto>
                {
                    new EmployeePayrollInfoDto
                    {
                        EmployeeName = "Juan Perez",
                        LegalId = "123456789",
                        EmployeeType = "Full-Time",
                        PaymentPeriod = "Mensual",
                        PaymentDate = "2024-06-30",
                        GrossSalary = 1000000,
                        EmployerSocialCharges = 150000,
                        VoluntaryDeductions = 20000,
                        EmployerCost = 1170000
                    }
                }
            };

            mockRepo.Setup(r => r.GetEmployeePayrollReport(companyId, employerId))
                .ReturnsAsync(expectedReport);

            var query = new GetEmployerEmployeePayrollReportQuery(mockRepo.Object);

            var result = await query.ExecuteAsync(companyId, employerId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CompanyName, Is.EqualTo("DentalCare"));
            Assert.That(result.EmployerName, Is.EqualTo("Dr. Smith"));
            Assert.That(result.Employees.Count, Is.EqualTo(1));
            Assert.That(result.Employees[0].EmployeeName, Is.EqualTo("Juan Perez"));
        }

        [Test]
        public async Task ExecuteAsync_ReturnsNull_WhenNoData()
        {
            var mockRepo = new Mock<IEmployerPayrollReportRepository>();
            var companyId = Guid.NewGuid();
            var employerId = Guid.NewGuid();

            mockRepo.Setup(r => r.GetEmployeePayrollReport(companyId, employerId))
                .ReturnsAsync((EmployerEmployeePayrollReportDto)null!);
            var query = new GetEmployerEmployeePayrollReportQuery(mockRepo.Object);

            var result = await query.ExecuteAsync(companyId, employerId);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ExecuteAsync_ThrowsException_WhenRepositoryFails()
        {
            var mockRepo = new Mock<IEmployerPayrollReportRepository>();
            var companyId = Guid.NewGuid();
            var employerId = Guid.NewGuid();

            mockRepo.Setup(r => r.GetEmployeePayrollReport(companyId, employerId))
                .ThrowsAsync(new InvalidOperationException("DB error"));

            var query = new GetEmployerEmployeePayrollReportQuery(mockRepo.Object);

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await query.ExecuteAsync(companyId, employerId));
        }

        [Test]
        public async Task ExecuteAsync_CallsRepository_WithCorrectParameters()
        {
            var mockRepo = new Mock<IEmployerPayrollReportRepository>();
            var companyId = Guid.NewGuid();
            var employerId = Guid.NewGuid();

            var query = new GetEmployerEmployeePayrollReportQuery(mockRepo.Object);

            await query.ExecuteAsync(companyId, employerId);

            mockRepo.Verify(r => r.GetEmployeePayrollReport(companyId, employerId), Times.Once);
        }
    }
}