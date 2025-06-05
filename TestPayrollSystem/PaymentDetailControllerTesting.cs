using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using backend.API.Controllers;
using backend.Application.Commands.PaymentDetails;
using backend.Application.Queries.PaymentDetails;
using backend.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace TestPaymentSystem
{
    public class PaymentDetailControllerTest
    {
        private Mock<ICreatePaymentDetailCommand> _mockCreateCommand;
        private Mock<IGetPaymentDetailByIdQuery> _mockGetByIdQuery;
        private Mock<IGetPaymentDetailsByEmployeeIdQuery> _mockGetByEmployeeIdQuery;
        private Mock<IGetPaymentDetailsByCompanyIdQuery> _mockGetByCompanyIdQuery;
        private PaymentDetailController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCreateCommand = new Mock<ICreatePaymentDetailCommand>();
            _mockGetByIdQuery = new Mock<IGetPaymentDetailByIdQuery>();
            _mockGetByEmployeeIdQuery = new Mock<IGetPaymentDetailsByEmployeeIdQuery>();
            _mockGetByCompanyIdQuery = new Mock<IGetPaymentDetailsByCompanyIdQuery>();

            _controller = new PaymentDetailController(
                _mockCreateCommand.Object,
                _mockGetByIdQuery.Object,
                _mockGetByEmployeeIdQuery.Object,
                _mockGetByCompanyIdQuery.Object);
        }

        [Test]
        public async Task GetByEmployeeId_ReturnsOkWithDetails_WhenDetailsExist()
        {
            var employeeId = Guid.NewGuid();
            var expectedDetails = new List<PaymentDetailModel> {
                new PaymentDetailModel {
                    Id = Guid.NewGuid(),
                    EmployeeId = employeeId,
                    GrossSalary = 1000,
                    IssueDate = DateTime.Now
                },
                new PaymentDetailModel {
                    Id = Guid.NewGuid(),
                    EmployeeId = employeeId,
                    GrossSalary = 1500,
                    IssueDate = DateTime.Now.AddDays(-1)
                }
            };

            _mockGetByEmployeeIdQuery.Setup(x => x.ExecuteAsync(employeeId))
                .ReturnsAsync(expectedDetails);

            var result = await _controller.GetByEmployeeId(employeeId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedDetails, result.Value);
        }


        [Test]
        public async Task GetByCompanyId_ReturnsOkWithDetails_WhenDetailsExist()
        {
            var companyId = Guid.NewGuid();
            var expectedDetails = new List<PaymentDetailModel> {
                new PaymentDetailModel {
                    Id = Guid.NewGuid(),
                    EmployeeId = Guid.NewGuid(),
                    GrossSalary = 1000,
                    IssueDate = DateTime.Now
                },
                new PaymentDetailModel {
                    Id = Guid.NewGuid(),
                    EmployeeId = Guid.NewGuid(),
                    GrossSalary = 1500,
                    IssueDate = DateTime.Now.AddDays(-1)
                }
            };

            _mockGetByCompanyIdQuery.Setup(x => x.ExecuteAsync(companyId))
                .ReturnsAsync(expectedDetails);

            var result = await _controller.GetByCompanyId(companyId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedDetails, result.Value);
        }

        [Test]
        public void PaymentDetailModel_Validation_ShouldPass_WhenAllRequiredFieldsPresent()
        {
            var model = new PaymentDetailModel
            {
                Id = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                GrossSalary = 1000,
                IssueDate = DateTime.Now
            };
            
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResults, true);
            
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

        [Test]
        public async Task GetById_ReturnsOk_WhenDetailExists()
        {
            var detailId = Guid.NewGuid();
            var expectedDetail = new PaymentDetailModel
            {
                Id = detailId,
                EmployeeId = Guid.NewGuid(),
                GrossSalary = 1200,
                IssueDate = DateTime.Now
            };

            _mockGetByIdQuery.Setup(x => x.ExecuteAsync(detailId))
                .ReturnsAsync(expectedDetail);

            var result = await _controller.GetById(detailId) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(expectedDetail, result.Value);
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenDetailDoesNotExist()
        {
            var detailId = Guid.NewGuid();

            _mockGetByIdQuery.Setup(x => x.ExecuteAsync(detailId))
                .ReturnsAsync((PaymentDetailModel)null!); 

            var result = await _controller.GetById(detailId) as NotFoundResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

    }
}