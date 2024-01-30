using System.Linq.Expressions;

namespace TalkBuddy.DAL.Interfaces;

public interface IGenericRepository<T> where T : class
{
    
    void Add(T entity);
    Task AddAsync(T entity);
    void AddMany(IEnumerable<T> entities);
    Task AddManyAsync(IEnumerable<T> entities);

    void Update(T entity);
    Task UpdateAsync(T entity);
    void UpdateMany(IEnumerable<T> entities);
    Task UpdateManyAsync(IEnumerable<T> entities);

    void DeleteMany(Expression<Func<T, bool>> predicate);
    Task DeleteManyAsync(Expression<Func<T, bool>> predicate);
    
    long Count(Expression<Func<T, bool>> predicate);
    Task<long> CountAsync(Expression<Func<T, bool>> predicate);
    
    bool Exists(Expression<Func<T, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}