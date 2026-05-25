using FitHub.Platform.Common.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace FitHub.Platform.Common.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<PaginatedResult<T>> GetPagedAsync(PageRequest pageRequest, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id);
        Task<int> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(Guid id);
    }

    public abstract class BaseRepository<T>(DbContext context) : IBaseRepository<T> where T : BaseEntity
    {
        protected DbSet<T> DbSet => context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
            => await DbSet.AsNoTracking().ToListAsync();

        public async Task<PaginatedResult<T>> GetPagedAsync(
            PageRequest pageRequest,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(pageRequest.SortBy))
                query = ApplyOrdering(query, pageRequest.SortBy, pageRequest.Descending);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = pageRequest.Page,
                PageSize = pageRequest.PageSize
            };
        }

        public async Task<T?> GetByIdAsync(Guid id)
            => await DbSet.FindAsync(id);

        public async Task<int> InsertAsync(T entity)
        {
            DbSet.Add(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            entity.ModifiedOn = DateTime.UtcNow;
            DbSet.Update(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity is null) return 0;
            DbSet.Remove(entity);
            return await context.SaveChangesAsync();
        }

        private static IQueryable<T> ApplyOrdering(IQueryable<T> query, string sortBy, bool descending)
        {
            var property = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                ?? throw new ArgumentException($"Property '{sortBy}' does not exist on '{typeof(T).Name}'.");

            var param = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(param, property);
            var lambda = Expression.Lambda(propertyAccess, param);

            var methodName = descending ? "OrderByDescending" : "OrderBy";
            var ordered = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(T), property.PropertyType],
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(ordered);
        }
    }
}
