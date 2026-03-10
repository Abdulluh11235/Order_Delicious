using System.Linq.Expressions;

namespace Domain.Interfaces.Repository;

public interface IRepository<T> where T : class
{
    void Add(T entity);
    Task<IEnumerable<T>> GetEntityPaged(int pageNumber, int pageSize);
    Task<T?> GetFirstOrDefault(Expression<Func<T,bool>> condition);
    public Task<IEnumerable<T>> FindEntityPaged(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
    public Task<int> Count();
    void RemoveRange(IEnumerable<T> entities);
    void Remove(T entity);
}   