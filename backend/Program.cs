using backend.Application.Commands.PaymentDetails;
using backend.Application.Queries.PaymentDetails;
using backend.Infraestructure;
using backend.Services;
using backend.Application;

var builder = WebApplication.CreateBuilder(args);
 
 builder.Services.AddCors(options =>
 {
     options.AddDefaultPolicy(policy =>
     {
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader();
     });
 });
 
 // Add services to the container.
 
 builder.Services.AddControllers();
 // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
 builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register Clean Architecture Services
builder.Services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
builder.Services.AddScoped<ICreatePaymentDetailCommand, CreatePaymentDetailCommand>();
builder.Services.AddScoped<IGetPaymentDetailByIdQuery, GetPaymentDetailByIdQuery>();
builder.Services.AddScoped<IGetPaymentDetailsByEmployeeIdQuery, GetPaymentDetailsByEmployeeIdQuery>();
builder.Services.AddScoped<IGetPaymentDetailsByCompanyIdQuery, GetPaymentDetailsByCompanyIdQuery>();

 builder.Services.AddSwaggerGen();
 
 builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
 builder.Services.AddScoped<IPayrollService, PayrollService>();

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