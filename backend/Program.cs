using backend.Application.Commands.PaymentDetails;
using backend.Application.Queries.PaymentDetails;
using backend.Application.Queries;
using backend.Application.Commands;
using backend.Application.GrossPaymentCalculation;
using backend.Application.Queries.Payroll;
using backend.Application.Commands.Payroll;
using backend.Application.Queries.Employees;
using backend.Application.Queries.Company;
using backend.Domain.Strategies;
using backend.Application;
using backend.Infraestructure;
using backend.Application.DeductionCalculation;
using System.Text.Json.Serialization;
using backend.Application.Queries.Payroll;
using backend.Application.Benefits.Commands;
using backend.Application.Benefits.Queries;
using backend.Application.Orchestrators.Deduction;
using backend.Application.Orchestrators.Payroll;
using backend.Application.Queries.Payroll;
using backend.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Clean Architecture Services
builder.Services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
builder.Services.AddScoped<ICreatePaymentDetailCommand, CreatePaymentDetailCommand>();
builder.Services.AddScoped<IGetPaymentDetailByIdQuery, GetPaymentDetailByIdQuery>();
builder.Services.AddScoped<IGetPaymentDetailsByEmployeeIdQuery, GetPaymentDetailsByEmployeeIdQuery>();
builder.Services.AddScoped<IGetPaymentDetailsByCompanyIdQuery, GetPaymentDetailsByCompanyIdQuery>();
builder.Services.AddScoped<IGetDaysByTimesheetIdQuery, GetDaysByTimesheetIdQuery>();
builder.Services.AddScoped<IGetEmployeeHoursInPeriodQuery, GetEmployeeHoursInPeriodQuery>();
builder.Services.AddScoped<IGetEmployeeTimesheetByDateQuery, GetEmployeeTimesheetByDateQuery>();
builder.Services.AddScoped<IUpdateDayCommand, UpdateDayCommand>();
builder.Services.AddScoped<IUpdatePayrollIdInTimesheetsCommand, UpdatePayrollIdInTimesheetsCommand>();
builder.Services.AddScoped<IInsertTimesheetsForPeriodCommand, InsertTimesheetsForPeriodCommand>();

builder.Services.AddScoped<IBenefitRepository, BenefitService>();

builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
builder.Services.AddScoped<IGetPayrollsByCompanyIdQuery, GetPayrollsByCompanyIdQuery>();
builder.Services.AddScoped<IGetPayrollsSummaryByCompanyIdQuery, GetPayrollsSummaryByCompanyIdQuery>();
builder.Services.AddScoped<ITimesheetRepository, TimesheetRepository>();
builder.Services.AddScoped<ICheckPayrollExistsQuery, CheckPayrollExistsQuery>();
builder.Services.AddScoped<IGetEmployeesByCompanyIdQuery, GetEmployeesByCompanyIdQuery>();
builder.Services.AddScoped<ICreatePayrollCommand, CreatePayrollCommand>();
builder.Services.AddScoped<IGetCompanyPaymentTypeByCompanyIdQuery, GetCompanyPaymentTypeByCompanyIdQuery>();
builder.Services.AddScoped<IPayrollOrchestrator, PayrollOrchestrator>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<GetBenefitsQuery>();
builder.Services.AddScoped<GetBenefitByIdQuery>();
builder.Services.AddScoped<CreateBenefitCommand>();
builder.Services.AddScoped<AssignBenefitsToEmployeeCommand>();
builder.Services.AddScoped<GetAssignedBenefitsQuery>();
builder.Services.AddScoped<UpdateBenefitCommand>();
builder.Services.AddScoped<IsBenefitAssignedQuery>();

builder.Services.AddScoped<IDeductionOrchestrator, DeductionOrchestrator>();

// Register Payment Calculation Strategies
builder.Services.AddScoped<MonthlyPaymentStrategy>();
builder.Services.AddScoped<BiweeklyPaymentStrategy>();
builder.Services.AddScoped<WeeklyPaymentStrategy>();

// Register Deduction Calculation Strategies
builder.Services.AddScoped<CcssDeductionStrategy>();
builder.Services.AddScoped<IncomeTaxDeductionStrategy>();
builder.Services.AddScoped<BenefitDeductionStrategy>();

// Register Calculation Orchestrator
builder.Services.AddScoped<DeductionCalculationOrchestrator>();
builder.Services.AddScoped<APIRepository>();
builder.Services.AddHttpClient<BenefitDeductionStrategy>();

builder.Services.AddScoped<IDeductionDetailRepository, DeductionDetailRepository>();
builder.Services.AddScoped<IInsertDeductionDetailsCommand, InsertDeductionDetailsCommand>();





// Register Strategy Orchestrator
builder.Services.AddScoped<GrossPaymentCalculationOrchestrator>();

builder.Services.AddScoped<ICalculateGrossPaymentQuery, CalculateGrossPaymentQuery>();
builder.Services.AddScoped<IDisableBenefitForEmployeeCommand, DisableBenefitForEmployeeCommand>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
