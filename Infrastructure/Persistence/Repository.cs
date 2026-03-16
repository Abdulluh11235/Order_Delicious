using System.Linq.Expressions;
using Domain.Interfaces;
using Domain.Interfaces.Repository;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public abstract class Repository<T>:IRepository<T> where T :class,IIdentifiable
{
    protected readonly AppDbContext Db;
    protected readonly DbSet<T> DbSet;
    public Repository(AppDbContext dbContext)
    {
        Db=dbContext;
        DbSet = Db.Set<T>();
    }

    public void Add(T entity)
    { 
        DbSet.Add(entity);
    }

    public virtual async Task<IEnumerable<T>> GetEntityPaged(int pageNumber, int pageSize)
    { 
        int offset = (pageNumber - 1) * pageSize;
      
        return await DbSet.Skip(offset).Take(pageSize).ToListAsync();
    }

    public virtual async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> condition)
    {
        return await DbSet.FirstOrDefaultAsync(condition);  
    }


    public virtual async Task<IEnumerable<T>> FindEntityPaged(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
    {
        return await DbSet.Where(predicate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<int> Count()
    {
        return await DbSet.CountAsync();
    }

    public virtual void Update(T entity)
    {
        DbSet.Update(entity);
    }
    public async Task RemoveRange(IEnumerable<int> ids)
    {
        await DbSet.Where(e => ids.Contains(e.Id))
            .ExecuteDeleteAsync();
    }
    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }
}