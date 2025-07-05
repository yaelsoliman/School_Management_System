using Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task<bool> AnyAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task<int> CountAsync();
        Task<T> GetByIdAsync(object id);
        Task<Pagination<TResult>> ToPagination<TResult>(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, object>>? orderBy = null,
            bool ascending = true,
            Expression<Func<T, TResult>> selector = null,
            string? searchTerm = null,
            Expression<Func<T, bool>>? searchPredicate = null);
        Task<T?> FirstOrDefaultAsync(
           Expression<Func<T, bool>> filter,
           Func<IQueryable<T>, IQueryable<T>>? include = null);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> sort, bool ascending = true);
        void Update(T entity);
        void Delete(T entity);

    }

}
