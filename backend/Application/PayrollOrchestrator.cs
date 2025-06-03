using backend.Domain;
using backend.Application.DTOs;
using backend.Application.Queries.Payroll;
using backend.Application.Queries.Employees;
using backend.Application.Commands.Payroll;
using backend.Application.Queries;
using backend.Application.GrossPaymentCalculation;
using backend.Application.Queries.Company;
using backend.Domain.Enums;
using backend.Application.Commands.PaymentDetails;
using backend.Application.Orchestrators.Deduction;
using backend.Application.Commands;
using backend.Application.Exceptions;

namespace backend.Application.Orchestrators.Payroll
{
    public interface IPayrollOrchestrator
    {
        Task<Guid> GeneratePayroll(PayrollModel model);
    }

    public class PayrollOrchestrator : IPayrollOrchestrator
    {
        private readonly ICheckPayrollExistsQuery _checkPayrollExistsQuery;
        private readonly IGetEmployeesByCompanyIdQuery _getEmployeesByCompanyIdQuery;
        private readonly ICreatePayrollCommand _createPayrollCommand;
        private readonly IGetEmployeeHoursInPeriodQuery _getEmployeeHoursInPeriodQuery;
        private readonly ICalculateGrossPaymentQuery _calculateGrossPaymentQuery;
        private readonly IGetCompanyPaymentTypeByCompanyIdQuery _getCompanyPaymentTypeByCompanyIdQuery;
        private readonly ICreatePaymentDetailCommand _createPaymentDetailCommand;
        private readonly IUpdatePayrollIdInTimesheetsCommand _updatePayrollIdInTimesheetsCommand;
        private readonly IDeductionOrchestrator _deductionOrchestrator;

        public PayrollOrchestrator(
            ICheckPayrollExistsQuery checkPayrollExistsQuery,
            IGetEmployeesByCompanyIdQuery getEmployeesByCompanyIdQuery,
            ICreatePayrollCommand createPayrollCommand,
            IGetEmployeeHoursInPeriodQuery getEmployeeHoursInPeriodQuery,
            ICalculateGrossPaymentQuery calculateGrossPaymentQuery,
            IGetCompanyPaymentTypeByCompanyIdQuery getCompanyPaymentTypeByCompanyIdQuery,
            ICreatePaymentDetailCommand createPaymentDetailCommand,
            IUpdatePayrollIdInTimesheetsCommand updatePayrollIdInTimesheetsCommand,
            IDeductionOrchestrator deductionOrchestrator)
        {
            _checkPayrollExistsQuery = checkPayrollExistsQuery;
            _getEmployeesByCompanyIdQuery = getEmployeesByCompanyIdQuery;
            _createPayrollCommand = createPayrollCommand;
            _getEmployeeHoursInPeriodQuery = getEmployeeHoursInPeriodQuery;
            _calculateGrossPaymentQuery = calculateGrossPaymentQuery;
            _getCompanyPaymentTypeByCompanyIdQuery = getCompanyPaymentTypeByCompanyIdQuery;
            _createPaymentDetailCommand = createPaymentDetailCommand;
            _updatePayrollIdInTimesheetsCommand = updatePayrollIdInTimesheetsCommand;
            _deductionOrchestrator = deductionOrchestrator;
        }

        public async Task<Guid> GeneratePayroll(PayrollModel model)
        {
            bool payrollExistsForPeriod = await _checkPayrollExistsQuery.ExecuteAsync(model.StartDate, model.EndDate, model.CompanyId);
            if (payrollExistsForPeriod) throw new PayrollException("AlreadyExists", "A payroll already exists for the given period.");


            var employees = await _getEmployeesByCompanyIdQuery.ExecuteAsync(model.CompanyId);
            if (employees.Count == 0) throw new PayrollException("NoEmployees", "No employees found for the given company.");

            var payrollId = await _createPayrollCommand.ExecuteAsync(model);

            var companyPaymentType = await _getCompanyPaymentTypeByCompanyIdQuery.ExecuteAsync(model.CompanyId);
            if (!Enum.TryParse<EmployeeTypePayment>(companyPaymentType, true, out var companyPaymentTypeEnum))
                throw new PayrollException("InvalidPaymentType", "Invalid payment type from company.");

            foreach (var employee in employees)
            {
                // This is required cause an employee can start working after the payroll start date
                var startDate = employee.EmployeeStartDate > model.StartDate ? employee.EmployeeStartDate : model.StartDate;

                _updatePayrollIdInTimesheetsCommand.Execute(payrollId, employee.Id, startDate, model.EndDate);

                var employeeHoursWorked = _getEmployeeHoursInPeriodQuery.Execute(employee.Id, startDate, model.EndDate);

                var grossPaymentDto = new CalculateGrossPaymentDto
                {
                    EmployeeTypePayment = companyPaymentTypeEnum,
                    BaseSalary = employee.GrossSalary,
                    StartDate = startDate,
                    EndDate = model.EndDate,
                    WorkedHours = employeeHoursWorked
                };

                var grossPayment = _calculateGrossPaymentQuery.Execute(grossPaymentDto);

                var paymentDetail = new PaymentDetailModel
                {
                    PayrollId = payrollId,
                    EmployeeId = employee.Id,
                    GrossSalary = grossPayment,
                    IssueDate = DateTime.UtcNow
                };

                var paymentDetailId = await _createPaymentDetailCommand.ExecuteAsync(paymentDetail);

                var deductionsDto = new CalculateDeductionDto
                {
                    EmployeeId = employee.Id,
                    GrossSalary = grossPayment,
                    PaymentDetailsId = paymentDetailId,
                    ContractType = employee.ContractType,
                    Gender = employee.Gender,
                };

                _deductionOrchestrator.CalculateDeductions(deductionsDto);
            }
            return payrollId;
        }
    }
}