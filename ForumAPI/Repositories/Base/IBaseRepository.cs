using System.Linq.Expressions;

namespace ForumApi.Repositories.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(Guid id);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> FirstOrDefaultIncludingWithoutTracking(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        void Create(TEntity entity);
        void CreateRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateMany(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetWhereWithoutTracking(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetWhereIncluding(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetWhereIncludingOrderBy(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>>? orderBy, bool descending,
            params Expression<Func<TEntity, object>>[] includeProperties);
        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate);
    }
}