using backend.Domain;
using backend.Infraestructure;
using backend.Application;

namespace backend.Application.Orchestrators.Payroll
{
    public interface IPayrollOrchestrator
    {
        Task<Guid> GeneratePayroll(Guid payrollManagerId, Guid companyId, DateTime startDate, DateTime endDate);
    }

    public class PayrollOrchestrator : IPayrollOrchestrator
    {
        private readonly IPayrollRepository _repository;

        
        public PayrollOrchestrator(IPayrollRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> GeneratePayroll(Guid payrollManagerId, Guid companyId, DateTime startDate, DateTime endDate)
        {
            // Step 0.0 Validate the endDate cannot be before the startDate we can validate this in the controller
            // Step 0 validate if the id match a payroll manager
            // step 0.1 validate if the period matches the payment period of the company

            // Step 1 validate there is not an existing payroll for the given period
            // use query approach neeed to create a new query for this
            var existingPayrolls = await _repository.ExistsPayrollForPeriodAsync(companyId, startDate, endDate);

            // Step 2 if there is an existing payroll, throw an exception


            // Maybe we should validate first we have employees before creating a payroll
            // Step 2.1 generate a new payroll row and gather the id

            // Step 3 get the list of employees for the company
            // generate a query

            // Step 4 get the worked hours for each employee in the given period
            // use existing query GetEmployeeHoursInPeriod

            // Step 5 calculate the payroll for each employee based on their worked hours and salary
            // use GrossPaymentCalculationQuery

            // Step 6 save the payment details in the database
            // use command create payment details 

            // Step 7 calculate deductions and taxes for each employee
            // leave a todo cause this will be implemented later

            // Step 8 save the deductions and taxes in the database
            // leave a todo cause this will be implemented later

            // Step 9 return the payroll id
            return await _repository.GeneratePayrollAsync(companyId, startDate, endDate);
        }
    }
}