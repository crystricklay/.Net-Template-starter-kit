using Template.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Template.Domain.Interfaces.Repositories.Base;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<Guid> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindManyAsync(Expression<Func<T, bool>> predicate);
}