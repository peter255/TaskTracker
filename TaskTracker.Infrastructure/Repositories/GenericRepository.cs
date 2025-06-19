using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Infrastructure.Persistence;

namespace TaskTracker.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity)
    {
        var result = await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
}
