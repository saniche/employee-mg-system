//generate employee repository class implementing IEmployeeRepository interface
using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeManagementAPI.Repositories
{
    //inherit from dbrepository and implement IEmployeeRepository
    public class EmployeeRepository : DbRepository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
