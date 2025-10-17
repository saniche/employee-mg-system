using EmployeeManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using EmployeeManagementAPI.Repositories;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Dtos;
using Mapster;
using FluentValidation;
using FluentValidation.AspNetCore;
using EmployeeManagementAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// register Mapster DI (Mapster.DependencyInjection)
builder.Services.AddMapster();
// Mapster global scan - looks for IRegister implementations in this assembly
TypeAdapterConfig.GlobalSettings.Scan(typeof(EmployeeManagementAPI.Mappings.MapsterConfig).Assembly);

// register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add controllers
builder.Services.AddControllers();

// add dbcontext service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));

//add cors to allow any origin, method, and header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

//add employee repository service
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// FluentValidation registration
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeCreateDtoValidator>();

var app = builder.Build();

// enable Swagger UI (commonly enabled in Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //make Swagger UI available at root URL        
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API V1");
        c.RoutePrefix = string.Empty; // Set the Swagger UI at the app's root
    });
}

app.InitialiseDatabaseAsync();

app.UseCors("AllowAll");

// map controllers
app.MapControllers();

app.Run();
