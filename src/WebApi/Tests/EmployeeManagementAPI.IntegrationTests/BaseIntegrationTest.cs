using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.IntegrationTests;

public abstract class BaseIntegrationTest
{
    protected TestWebApplicationFactory Factory { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        Factory = factory;
    }

    protected HttpClient CreateClient() => Factory.CreateClient();

    protected async Task SeedAsync(params Employee[] employees)
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Employees.AddRange(employees);
        await db.SaveChangesAsync();
    }

    protected async Task ClearEmployeesAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Employees.RemoveRange(db.Employees);
        await db.SaveChangesAsync();
    }
}
