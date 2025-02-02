using Microsoft.EntityFrameworkCore;
using ComplianceDashboard.Infrastructure.Data;

namespace ComplianceDashboard.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<T> DbSet;

        protected BaseRepository(ApplicationDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var entry = await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entry.Entity;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            var affected = await Context.SaveChangesAsync();
            return affected > 0;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            DbSet.Remove(entity);
            var affected = await Context.SaveChangesAsync();
            return affected > 0;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await DbSet.FindAsync(id) != null;
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public virtual async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync() > 0;
        }

        protected virtual IQueryable<T> ApplyPaging(IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}