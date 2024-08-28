using YourNuts.Domain;

namespace Domain
{
    public interface IRepository<TEntity> where TEntity : class
    {
        YourNutsDbContext dbContext { get; set; }

        IEnumerable<TEntity> GetAll();
        string IdField { get; set; }

        TEntity? Get(object id);

        Task<TEntity?> GetAsync(object id, CancellationToken cancellationToken = default);

        TEntity? Create(TEntity entity);

        Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        
        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Delete(object id);

        Task DeleteAsync(object id, CancellationToken cancellationToken = default);

        IList<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> query);

        Task<IList<TEntity>> QueryAsync(
            Func<IQueryable<TEntity>, IQueryable<TEntity>> query, CancellationToken cancellationToken = default);
    }
}