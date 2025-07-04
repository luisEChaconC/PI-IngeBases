using System;
using System.Threading.Tasks;
using backend.API;
using backend.Application.Exceptions;
using backend.Application.Orchestrators.Payroll;
using backend.Application.Queries.Payroll;
using backend.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestPayrollSystem
{
    public class PayrollControllerTest
    {
        private Mock<IPayrollOrchestrator> _mockOrchestrator;
        private Mock<IGetPayrollsByCompanyIdQuery> _mockGetByCompanyId;
        private Mock<IGetPayrollsSummaryByCompanyIdQuery> _mockGetSummary;
        private PayrollController _controller;

        [SetUp]
        public void Setup()
        {
            _mockOrchestrator = new Mock<IPayrollOrchestrator>();
            _mockGetByCompanyId = new Mock<IGetPayrollsByCompanyIdQuery>();
            _mockGetSummary = new Mock<IGetPayrollsSummaryByCompanyIdQuery>();
            _controller = new PayrollController(_mockOrchestrator.Object, _mockGetByCompanyId.Object, _mockGetSummary.Object);
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenCompanyIdIsEmpty()
        {
            var model = new PayrollModel { CompanyId = Guid.Empty };
            var result = await _controller.Create(model);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenPayrollManagerIdIsEmpty()
        {
            var model = new PayrollModel { CompanyId = Guid.NewGuid(), PayrollManagerId = Guid.Empty };
            var result = await _controller.Create(model);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenStartDateIsDefault()
        {
            var model = new PayrollModel
            {
                CompanyId = Guid.NewGuid(),
                PayrollManagerId = Guid.NewGuid(),
                StartDate = default
            };
            var result = await _controller.Create(model);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenEndDateIsDefault()
        {
            var model = new PayrollModel
            {
                CompanyId = Guid.NewGuid(),
                PayrollManagerId = Guid.NewGuid(),
                StartDate = DateTime.Today,
                EndDate = default
            };
            var result = await _controller.Create(model);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenEndDateIsNotGreaterThanStartDate()
        {
            var model = new PayrollModel
            {
                CompanyId = Guid.NewGuid(),
                PayrollManagerId = Guid.NewGuid(),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            };
            var result = await _controller.Create(model);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_ReturnsServerError_OnException()
        {
            var model = new PayrollModel
            {
                CompanyId = Guid.NewGuid(),
                PayrollManagerId = Guid.NewGuid(),
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };
            _mockOrchestrator.Setup(x => x.GeneratePayroll(It.IsAny<PayrollModel>()))
                .ThrowsAsync(new Exception("fail"));

            var result = await _controller.Create(model) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Test]
        public async Task GetByCompanyId_ReturnsBadRequest_WhenCompanyIdIsEmpty()
        {
            var result = await _controller.GetByCompanyId(Guid.Empty);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetByCompanyId_ReturnsServerError_OnException()
        {
            var companyId = Guid.NewGuid();
            _mockGetByCompanyId.Setup(x => x.ExecuteAsync(companyId)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.GetByCompanyId(companyId) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Test]
        public async Task GetSummaryByCompanyId_ReturnsBadRequest_WhenCompanyIdIsEmpty()
        {
            var result = await _controller.GetSummaryByCompanyId(Guid.Empty);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetSummaryByCompanyId_ReturnsServerError_OnException()
        {
            var companyId = Guid.NewGuid();
            _mockGetSummary.Setup(x => x.ExecuteAsync(companyId)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.GetSummaryByCompanyId(companyId) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}