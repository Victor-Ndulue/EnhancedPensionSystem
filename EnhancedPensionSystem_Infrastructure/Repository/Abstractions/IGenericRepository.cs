using System.Linq.Expressions;

namespace EnhancedPensionSystem_Infrastructure.Repository.Implementations;

public interface IGenericRepository<T> where T : class
{
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task BeginTransactionAsync();
    Task<bool> ExistsByConditionAsync(Expression<Func<T, bool>> condition);
    IQueryable<T> GetAll();
    IQueryable<T> GetByCondition(Expression<Func<T, bool>> conditionExpression);
    IQueryable<T> GetAllNonDeleted();
    IQueryable<T> GetNonDeletedByCondition(Expression<Func<T, bool>> condition);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Delete(T entity);
    void SoftDelete(T entity);
    void SoftDeleteRange(IEnumerable<T> entities);
    Task SaveChangesAsync();
}
