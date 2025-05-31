using backend.Application.Commands.PaymentDetails;
using backend.Application.Queries.PaymentDetails;
using backend.Application.Queries;
using backend.Application.GrossPaymentCalculation;
using backend.Domain.Strategies;
using backend.Services;
using backend.Application;
using backend.Infraestructure;
using backend.Application.GrossPaymentCalculation;
using System.Text.Json.Serialization;

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

builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<ITimesheetRepository, TimesheetRepository>();


// Register Payment Calculation Strategies
builder.Services.AddScoped<MonthlyPaymentStrategy>();
builder.Services.AddScoped<BiweeklyPaymentStrategy>();
builder.Services.AddScoped<WeeklyPaymentStrategy>();


// Register Strategy Orchestrator
builder.Services.AddScoped<GrossPaymentCalculationOrchestrator>();

builder.Services.AddScoped<ICalculateGrossPaymentQuery, CalculateGrossPaymentQuery>();


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
