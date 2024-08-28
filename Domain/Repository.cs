using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourNuts.Domain;

namespace Domain
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public YourNutsDbContext dbContext { get; set; }
        public string IdField { get; set; } = "id";

        public Repository(YourNutsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().ToList();
        }

        public TEntity? Get(object id)
        {
            return dbContext.Set<TEntity>().Find(id);
        }

       public async Task<TEntity?> GetAsync(object id, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(id.ToString());

            var entity = await dbContext.Set<TEntity>().FindAsync(id, cancellationToken);

            return entity;
        }
        public TEntity? Create(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges();

            return entity;
        }

        public async Task<TEntity?> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
            dbContext.SaveChanges(); ;
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            dbContext.Set<TEntity>().Update(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Delete(object id)
        {
            var entity = Get(id);

            if (entity == null) return;

            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();           
        }

        public async Task DeleteAsync(object id, CancellationToken cancellationToken = default)
        {

            var entity = await GetAsync(id, cancellationToken);

            if (entity != null)
            {
                dbContext.Set<TEntity>().Remove(entity);
                await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public IList<TEntity> Query(Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        {
           
            return query(dbContext.Set<TEntity>()).ToList();
        }

        public async Task<IList<TEntity>> QueryAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> query, CancellationToken cancellationToken = default)
        {
            return await query(dbContext.Set<TEntity>()).ToListAsync(cancellationToken);
        }
    }
}
