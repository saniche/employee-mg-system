//Generate Irepository interface for generic CRUD operations, with pagination support.
using System.Linq.Expressions;

namespace EmployeeManagementAPI.Repositories
{
    public interface IDbRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize);
    }
}