using Application.Repository;
using Common;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {
        protected DbSet<T> _dbSet = context.Set<T>();

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }


        #region  Read

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbSet.AnyAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return filter == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(filter);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Pagination<TResult>> ToPagination<TResult>(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>> selector = null,
             string? searchTerm = null,
            Expression<Func<T, bool>>? searchPredicate = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm) && searchPredicate != null)
            {
                query = query.Where(searchPredicate);
            }

            orderBy ??= x => EF.Property<object>(x, "Id");

            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            var projectedQuery = query.Select(selector);

            var result = await Pagination<TResult>.ToPagedList(projectedQuery, pageIndex, pageSize);

            return result;
        }

        public async Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> filter,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet.IgnoreQueryFilters().AsNoTracking();

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> sort, bool ascending = true)
        {
            var query = _dbSet.IgnoreQueryFilters()
                              .AsNoTracking()
                              .Where(filter);

            query = ascending ? query.OrderBy(sort) : query.OrderByDescending(sort);

            return await query.FirstOrDefaultAsync();
        }

        #endregion
        #region Update & delete

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        #endregion
    }

}
