
using DataAccess.Data;
using DataAccess.Interface;
using Entity.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace DataAccess.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private int? count;
        protected readonly BookWebStoreDbContext _dbContext;
        private readonly DbSet<T> _dbSet;   

        public Repository(BookWebStoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false) // Comma separated, Case Sensitive (e.g. "Category,Product") include properties
        {
            IQueryable<T> dbSetQuery = tracked ? _dbSet : _dbSet.AsNoTracking();
            if (filter != null)
                dbSetQuery = dbSetQuery.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbSetQuery = dbSetQuery.Include(includeProperty);
                }
            }
            return await dbSetQuery.ToListAsync();
        }
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false) // Comma separated, Case Sensitive (e.g. "Category,Product") include properties
        {
            IQueryable<T> dbSetQuery = tracked ? _dbSet : _dbSet.AsNoTracking();
            if (filter != null)
                dbSetQuery = dbSetQuery.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbSetQuery = dbSetQuery.Include(includeProperty);
                }
            }
            return await dbSetQuery.FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> ListAllAsync(QueryOptions<T> options) =>
        await builderQuery(options).ToListAsync();
        // if count is null (where is not use) then use _dbset.CountAsync()
        public async Task<int> CountAsync()=>count??await _dbSet.CountAsync();
        public virtual async Task<T?> GetAsync(Guid id) =>
            await _dbSet.FindAsync(id);
        public virtual async Task<T?> GetAsync(string id) =>
            await _dbSet.FindAsync(id);
        public virtual async Task<T?> GetAsync(QueryOptions<T> options) =>
            await builderQuery(options).FirstOrDefaultAsync();
        public void Add(T entity) => _dbSet.Add(entity);
        public void AddRange(List<T> values) => _dbSet.AddRange(values);

        public void Remove(T entity) => _dbSet.Remove(entity);
        public void RemoveRange(T entity) => _dbSet.RemoveRange(entity);
        public void RemoveRange(List<T> entities) => _dbSet.RemoveRange(entities);
        private IQueryable<T> builderQuery (QueryOptions<T> opt)
        {
            IQueryable<T> query = _dbSet;
            if (opt.HasInclude)
            {
                foreach(string include in opt.getIncludes())
                {
                    query = query.Include(include);
                }
            }
            if(opt.HasWhere)
            {
                foreach(var express in opt.WhereClauses)
                {
                    query = query.Where(express);
                }
                count = query.Count();  
            }
            if(opt.HasOrderBy)
            {
                if (opt.OrderByDirection == "asc")
                {
                    query = query.OrderBy(opt.OrderBy);
                }
                else
                {
                    query = query.OrderByDescending(opt.OrderBy);
                }
            }
            if (opt.HasPaging)
            {
                query = query.PageBy(opt.PageNumber, opt.PageSize);
            }

            return query;
        }
    }
}
