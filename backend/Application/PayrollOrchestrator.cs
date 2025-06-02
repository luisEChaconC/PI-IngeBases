using backend.Domain;
using backend.Infraestructure;
using backend.Application.Queries.Payroll;

namespace backend.Application.Orchestrators.Payroll
{
    public interface IPayrollOrchestrator
    {
        Task<Guid> GeneratePayroll(PayrollModel model);
    }

    public class PayrollOrchestrator : IPayrollOrchestrator
    {
        private readonly IPayrollRepository _repository;
        private readonly ICheckPayrollExistsQuery _checkPayrollExistsQuery;
        
        public PayrollOrchestrator(IPayrollRepository repository, ICheckPayrollExistsQuery checkPayrollExistsQuery)
        {
            _repository = repository;
            _checkPayrollExistsQuery = checkPayrollExistsQuery;
        }

        public async Task<Guid> GeneratePayroll(PayrollModel model)
        {
            // Step 0 validate if the id match a payroll manager // veremos
            // step 0.1 validate if the period matches the payment period of the company // veremos

            // Step 1 validate there is not an existing payroll for the given period
            bool payrollExistsForPeriod = await _checkPayrollExistsQuery.ExecuteAsync(model.StartDate, model.EndDate, model.CompanyId);
            // Step 2 if there is an existing payroll, throw an exception
            if (payrollExistsForPeriod) throw new InvalidOperationException("A payroll already exists for the given period.");

            // Maybe we should validate first we have employees before creating a payroll
            // Step 2.1 generate a new payroll row and gather the id

            // Step 3 get the list of employees for the company
            // generate a query

            // Step 4 get the worked hours for each employee in the given period
            // use existing query GetEmployeeHoursInPeriod

            // Step 5 calculate the payroll for each employee based on their worked hours and salary
            // for gross salary calculation assume the startDate is the employeeStartDate if the employee StartDate is after the startDate of the payroll
            // use GrossPaymentCalculationQuery

            // Step 6 save the payment details in the database
            // use command create payment details 

            // Step 7 calculate deductions and taxes for each employee
            // leave a todo cause this will be implemented later

            // Step 8 save the deductions and taxes in the database
            // leave a todo cause this will be implemented later

            // Step 9 return the payroll id
            //return await _repository.GeneratePayrollAsync(companyId, startDate, endDate);
            return new Guid();
        }
    }
}