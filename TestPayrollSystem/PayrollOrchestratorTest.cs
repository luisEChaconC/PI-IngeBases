using AutoFixture;
using backend.Application.Commands;
using backend.Application.Commands.PaymentDetails;
using backend.Application.Commands.Payroll;
using backend.Application.DTOs;
using backend.Application.GrossPaymentCalculation;
using backend.Application.Orchestrators.Deduction;
using backend.Application.Orchestrators.Payroll;
using backend.Application.Queries;
using backend.Application.Queries.Company;
using backend.Application.Queries.Employees;
using backend.Application.Queries.Payroll;
using backend.Application.Exceptions;
using backend.Domain;
using Moq;
using NUnit.Framework;


namespace TestPayrollSystem
{
    [TestFixture]
    public class PayrollOrchestratorTest
    {
        private Mock<ICheckPayrollExistsQuery> _checkPayrollExistsQuery;
        private Mock<IGetEmployeesByCompanyIdQuery> _getEmployeesByCompanyIdQuery;
        private Mock<ICreatePayrollCommand> _createPayrollCommand;
        private Mock<IGetEmployeeHoursInPeriodQuery> _getEmployeeHoursInPeriodQuery;
        private Mock<ICalculateGrossPaymentQuery> _calculateGrossPaymentQuery;
        private Mock<IGetCompanyPaymentTypeByCompanyIdQuery> _getCompanyPaymentTypeByCompanyIdQuery;
        private Mock<ICreatePaymentDetailCommand> _createPaymentDetailCommand;
        private Mock<IUpdatePayrollIdInTimesheetsCommand> _updatePayrollIdInTimesheetsCommand;
        private Mock<IInsertTimesheetsForPeriodCommand> _insertTimesheetsForPeriodCommand;
        private Mock<IDeductionOrchestrator> _deductionOrchestrator;
        private PayrollOrchestrator _sut;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _checkPayrollExistsQuery = new Mock<ICheckPayrollExistsQuery>();
            _getEmployeesByCompanyIdQuery = new Mock<IGetEmployeesByCompanyIdQuery>();
            _createPayrollCommand = new Mock<ICreatePayrollCommand>();
            _getEmployeeHoursInPeriodQuery = new Mock<IGetEmployeeHoursInPeriodQuery>();
            _calculateGrossPaymentQuery = new Mock<ICalculateGrossPaymentQuery>();
            _getCompanyPaymentTypeByCompanyIdQuery = new Mock<IGetCompanyPaymentTypeByCompanyIdQuery>();
            _createPaymentDetailCommand = new Mock<ICreatePaymentDetailCommand>();
            _updatePayrollIdInTimesheetsCommand = new Mock<IUpdatePayrollIdInTimesheetsCommand>();
            _insertTimesheetsForPeriodCommand = new Mock<IInsertTimesheetsForPeriodCommand>();
            _deductionOrchestrator = new Mock<IDeductionOrchestrator>();
            _sut = new PayrollOrchestrator(
                _checkPayrollExistsQuery.Object,
                _getEmployeesByCompanyIdQuery.Object,
                _createPayrollCommand.Object,
                _getEmployeeHoursInPeriodQuery.Object,
                _calculateGrossPaymentQuery.Object,
                _getCompanyPaymentTypeByCompanyIdQuery.Object,
                _createPaymentDetailCommand.Object,
                _updatePayrollIdInTimesheetsCommand.Object,
                _insertTimesheetsForPeriodCommand.Object,
                _deductionOrchestrator.Object
            );
            _fixture = new Fixture();
        }

        [Test]
        public void GeneratePayroll_ThrowsException_WhenPayrollAlreadyExists()
        {
            var model = _fixture.Create<PayrollModel>();
            _checkPayrollExistsQuery.Setup(x => x.ExecuteAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(true);

            var ex = Assert.Throws<PayrollException>(() => _sut.GeneratePayroll(model).GetAwaiter().GetResult());
            Assert.AreEqual("A payroll already exists for the given period.", ex.Message);
            Assert.AreEqual("AlreadyExists", ex.ErrorType);
        }

        [Test]
        public void GeneratePayroll_ThrowsException_WhenNoEmployees()
        {
            var model = _fixture.Create<PayrollModel>();
            _checkPayrollExistsQuery.Setup(x => x.ExecuteAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(false);
            _getEmployeesByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync(new List<EmployeeSummaryDto>());

            var ex = Assert.Throws<PayrollException>(() => _sut.GeneratePayroll(model).GetAwaiter().GetResult());
            Assert.AreEqual("No employees found for the given company.", ex.Message);
            Assert.AreEqual("NoEmployees", ex.ErrorType);
        }

        [Test]
        public void GeneratePayroll_ThrowsException_WhenInvalidPaymentType()
        {
            var model = _fixture.Create<PayrollModel>();
            _checkPayrollExistsQuery.Setup(x => x.ExecuteAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(false);
            _getEmployeesByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync(GetEmployeeSummaries());
            _createPayrollCommand.Setup(x => x.ExecuteAsync(model)).ReturnsAsync(Guid.NewGuid());
            _getCompanyPaymentTypeByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync("InvalidType");

            var ex = Assert.Throws<PayrollException>(() => _sut.GeneratePayroll(model).GetAwaiter().GetResult());
            Assert.AreEqual("Invalid payment type from company.", ex.Message);
            Assert.AreEqual("InvalidPaymentType", ex.ErrorType);
        }

        [Test]
        public void GeneratePayroll_SkipsEmployees_OutsidePayrollPeriod()
        {
            var model = _fixture.Build<PayrollModel>()
                .With(x => x.StartDate, new DateTime(2024, 1, 1))
                .With(x => x.EndDate, new DateTime(2024, 1, 31))
                .With(x => x.CompanyId, Guid.NewGuid())
                .Create();

            var employees = new List<EmployeeSummaryDto>
            {
                _fixture.Build<EmployeeSummaryDto>()
                    .With(x => x.EmployeeStartDate, new DateTime(2024, 2, 1))
                    .With(x => x.EndDate, (DateTime?)null)
                    .Create(),
                _fixture.Build<EmployeeSummaryDto>()
                    .With(x => x.EmployeeStartDate, new DateTime(2023, 1, 1))
                    .With(x => x.EndDate, new DateTime(2023, 12, 31))
                    .Create(),
                _fixture.Build<EmployeeSummaryDto>()
                    .With(x => x.EmployeeStartDate, new DateTime(2023, 1, 1))
                    .With(x => x.EndDate, (DateTime?)null)
                    .Create()
            };

            _checkPayrollExistsQuery.Setup(x => x.ExecuteAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(false);
            _getEmployeesByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync(employees);
            _createPayrollCommand.Setup(x => x.ExecuteAsync(model)).ReturnsAsync(Guid.NewGuid());
            _getCompanyPaymentTypeByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync("Monthly");
            _updatePayrollIdInTimesheetsCommand.Setup(x => x.Execute(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            _getEmployeeHoursInPeriodQuery.Setup(x => x.Execute(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(40);
            _calculateGrossPaymentQuery.Setup(x => x.Execute(It.IsAny<CalculateGrossPaymentDto>())).Returns(1000m);
            _createPaymentDetailCommand.Setup(x => x.ExecuteAsync(It.IsAny<PaymentDetailModel>())).ReturnsAsync(Guid.NewGuid());

            _sut.GeneratePayroll(model).GetAwaiter().GetResult();

            _updatePayrollIdInTimesheetsCommand.Verify(x => x.Execute(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Test]
        public void GeneratePayroll_CallsInsertTimesheetsForPeriodCommand_ForWeeklyActiveEmployees()
        {
            var model = _fixture.Build<PayrollModel>()
                .With(x => x.StartDate, new DateTime(2024, 1, 1))
                .With(x => x.EndDate, new DateTime(2024, 1, 7))
                .With(x => x.CompanyId, Guid.NewGuid())
                .Create();

            var employee = _fixture.Build<EmployeeSummaryDto>()
                .With(x => x.EmployeeStartDate, new DateTime(2023, 1, 1))
                .With(x => x.EndDate, (DateTime?)null)
                .With(x => x.IsDeleted, false)
                .Create();

            _checkPayrollExistsQuery.Setup(x => x.ExecuteAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(false);
            _getEmployeesByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync(new List<EmployeeSummaryDto> { employee });
            _createPayrollCommand.Setup(x => x.ExecuteAsync(model)).ReturnsAsync(Guid.NewGuid());
            _getCompanyPaymentTypeByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync("Weekly");
            _updatePayrollIdInTimesheetsCommand.Setup(x => x.Execute(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            _getEmployeeHoursInPeriodQuery.Setup(x => x.Execute(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(40);
            _calculateGrossPaymentQuery.Setup(x => x.Execute(It.IsAny<CalculateGrossPaymentDto>())).Returns(1000m);
            _createPaymentDetailCommand.Setup(x => x.ExecuteAsync(It.IsAny<PaymentDetailModel>())).ReturnsAsync(Guid.NewGuid());

            _sut.GeneratePayroll(model).GetAwaiter().GetResult();

            _insertTimesheetsForPeriodCommand.Verify(x =>
                x.Execute(
                    model.StartDate + TimeSpan.FromDays(7),
                    model.EndDate + TimeSpan.FromDays(7),
                    employee.Id,
                    It.IsAny<Guid>()),
                Times.Once);
        }

        [Test]
        public void GeneratePayroll_ReturnsPayrollId_WhenAllIsValid()
        {
            var model = _fixture.Build<PayrollModel>()
                .With(x => x.CompanyId, Guid.NewGuid())
                .Create();

            var employees = GetEmployeeSummaries();

            _checkPayrollExistsQuery.Setup(x => x.ExecuteAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(false);
            _getEmployeesByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync(employees);
            var payrollId = Guid.NewGuid();
            _createPayrollCommand.Setup(x => x.ExecuteAsync(model)).ReturnsAsync(payrollId);
            _getCompanyPaymentTypeByCompanyIdQuery.Setup(x => x.ExecuteAsync(It.IsAny<Guid>())).ReturnsAsync("Monthly");
            _updatePayrollIdInTimesheetsCommand.Setup(x => x.Execute(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            _getEmployeeHoursInPeriodQuery.Setup(x => x.Execute(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(40);
            _calculateGrossPaymentQuery.Setup(x => x.Execute(It.IsAny<CalculateGrossPaymentDto>())).Returns(1000m);
            _createPaymentDetailCommand.Setup(x => x.ExecuteAsync(It.IsAny<PaymentDetailModel>())).ReturnsAsync(Guid.NewGuid());

            var result = _sut.GeneratePayroll(model).GetAwaiter().GetResult();

            Assert.AreEqual(payrollId, result);
        }

        private List<EmployeeSummaryDto> GetEmployeeSummaries()
        {
            return _fixture.CreateMany<EmployeeSummaryDto>(2).ToList();
        }
    }
}