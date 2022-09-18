using System.Reflection;
using CompanyManagement.Core.Abstractions.OrganizationImporters;
using CompanyManagement.Core.Abstractions.Services;
using CompanyManagement.Core.OrganizationImporters;
using CompanyManagement.Core.Services;
using CompanyManagement.Migrations;
using CompanyManagement.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
            assembly => 
                assembly.MigrationsAssembly(Assembly.GetAssembly(typeof(MigrationsEntryPoint)).FullName));
});

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IOrganizationImporterFromFile, OrganizationImporterFromExcel>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();