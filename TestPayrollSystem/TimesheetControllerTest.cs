using System;
using System.Threading.Tasks;
using backend.API;
using backend.Application.Commands;
using backend.Application.DTOs;
using backend.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace TestPayrollSystem
{
    public class TimesheetControllerTest
    {
        private Mock<IGetDaysByTimesheetIdQuery> _mockGetDaysByTimesheetIdQuery;
        private Mock<IGetEmployeeHoursInPeriodQuery> _mockGetEmployeeHoursInPeriodQuery;
        private Mock<IGetEmployeeTimesheetByDateQuery> _mockGetEmployeeTimesheetByDateQuery;
        private Mock<IUpdateDayCommand> _mockUpdateDayCommand;
        private Mock<IUpdatePayrollIdInTimesheetsCommand> _mockUpdatePayrollIdInTimesheetsCommand;
        private Mock<IInsertTimesheetsForPeriodCommand> _mockInsertTimesheetsForPeriodCommand;
        private TimesheetController _controller;

        [SetUp]
        public void Setup()
        {
            _mockGetDaysByTimesheetIdQuery = new Mock<IGetDaysByTimesheetIdQuery>();
            _mockGetEmployeeHoursInPeriodQuery = new Mock<IGetEmployeeHoursInPeriodQuery>();
            _mockGetEmployeeTimesheetByDateQuery = new Mock<IGetEmployeeTimesheetByDateQuery>();
            _mockUpdateDayCommand = new Mock<IUpdateDayCommand>();
            _mockUpdatePayrollIdInTimesheetsCommand = new Mock<IUpdatePayrollIdInTimesheetsCommand>();
            _mockInsertTimesheetsForPeriodCommand = new Mock<IInsertTimesheetsForPeriodCommand>();

            _controller = new TimesheetController(
                _mockGetDaysByTimesheetIdQuery.Object,
                _mockGetEmployeeHoursInPeriodQuery.Object,
                _mockUpdatePayrollIdInTimesheetsCommand.Object,
                _mockGetEmployeeTimesheetByDateQuery.Object,
                _mockUpdateDayCommand.Object,
                _mockInsertTimesheetsForPeriodCommand.Object
            );
        }

        [Test]
        public async Task GetDaysByTimesheetId_ReturnsBadRequest_WhenTimesheetIdIsEmpty()
        {
            var result = _controller.GetDaysByTimesheetId(Guid.Empty);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetDaysByTimesheetId_ReturnsServerError_OnException()
        {
            var timesheetId = Guid.NewGuid();
            _mockGetDaysByTimesheetIdQuery.Setup(x => x.Execute(timesheetId)).Throws(new Exception("error"));

            var result = _controller.GetDaysByTimesheetId(timesheetId) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Test]
        public async Task GetEmployeeHoursInPeriod_ReturnsBadRequest_WhenEmployeeIdIsEmpty()
        {
            var result = _controller.GetEmployeeHoursInPeriod(Guid.Empty, DateTime.Today, DateTime.Today.AddDays(1));
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetEmployeeHoursInPeriod_ReturnsBadRequest_WhenEndDateIsNotGreaterThanStartDate()
        {
            var result = _controller.GetEmployeeHoursInPeriod(Guid.NewGuid(), DateTime.Today.AddDays(1), DateTime.Today);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetEmployeeHoursInPeriod_ReturnsServerError_OnException()
        {
            var employeeId = Guid.NewGuid();
            var startDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(1);
            _mockGetEmployeeHoursInPeriodQuery.Setup(x => x.Execute(employeeId, startDate, endDate)).Throws(new Exception("error"));

            var result = _controller.GetEmployeeHoursInPeriod(employeeId, startDate, endDate) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Test]
        public async Task GetEmployeeTimesheetByDate_ReturnsBadRequest_WhenEmployeeIdIsEmpty()
        {
            var result = _controller.GetEmployeeTimesheetByDate(Guid.Empty, DateTime.Today);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task GetEmployeeTimesheetByDate_ReturnsServerError_OnException()
        {
            var employeeId = Guid.NewGuid();
            var date = DateTime.Today;
            _mockGetEmployeeTimesheetByDateQuery.Setup(x => x.Execute(employeeId, date)).Throws(new Exception("error"));

            var result = _controller.GetEmployeeTimesheetByDate(employeeId, date) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, result.StatusCode);
        }
    }
}

