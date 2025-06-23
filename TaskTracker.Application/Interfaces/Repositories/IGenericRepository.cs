using System.Linq.Expressions;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<TType>> GetSpecificAsync<TType>(Expression<Func<T, TType>> select, Expression<Func<T, bool>> filter = null, int take = 0, int skip = 0);
    Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> SaveChangesAsync();
}
