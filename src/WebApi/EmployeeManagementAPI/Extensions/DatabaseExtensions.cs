//add extension medthod to migrate and seed database
using EmployeeManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

public static class DatabaseExtensions
{
    public static WebApplication InitialiseDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
            SeedAsync(dbContext).GetAwaiter().GetResult();
        }
        return app;
    }

    private static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Employees.Any())
        {
            context.Employees.AddRange(
                new EmployeeManagementAPI.Models.Employee
                {
                    FirstName = "Cheicky",
                    LastName = "Saniche",
                    Email = "csaniche@example.com",
                    PhoneNumber = "123-456-7890",
                    Position = "Software Engineer",
                    Salary = 80000m
                }
                // add 4 more employees
                , new EmployeeManagementAPI.Models.Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "jdoe@example.com",
                    PhoneNumber = "123-456-7890",
                    Position = "Software Engineer",
                    Salary = 80000m
                },
                new EmployeeManagementAPI.Models.Employee
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jsmith@example.com",
                    PhoneNumber = "123-456-7890",
                    Position = "Product Manager",
                    Salary = 90000m
                },
                new EmployeeManagementAPI.Models.Employee
                {
                    FirstName = "Michael",
                    LastName = "Johnson",
                    Email = "mjohnson@example.com",
                    PhoneNumber = "123-456-7890",
                    Position = "UX Designer",
                    Salary = 70000m
                },
                new EmployeeManagementAPI.Models.Employee
                {
                    FirstName = "Emily",
                    LastName = "Davis",
                    Email = "edavis@example.com",
                    PhoneNumber = "123-456-7890",
                    Position = "QA Engineer",
                    Salary = 60000m
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
