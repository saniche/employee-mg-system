//Generate IEmployeeRepository interface for employee-specific data access
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Repositories
{
    public interface IEmployeeRepository : IDbRepository<Employee>
    {
        Task<Employee> GetByEmailAsync(string email);
    }
}