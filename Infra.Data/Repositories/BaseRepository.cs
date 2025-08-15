using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    internal class BaseRepository<T>(IUnitOfWork unitOfWork) : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet = unitOfWork.Context.Set<T>();
        public IQueryable<T> GetAll() => _dbSet.AsQueryable();
        public IQueryable<T> Get(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).AsQueryable();
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate) => await Get(predicate).ToListAsync();
        public async Task<T> GetAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) throw new ArgumentException("Entity not found");
            return entity;
        }
        public async Task<IEnumerable<T>> GetNoTrackingAsync(Expression<Func<T, bool>> predicate) => await Get(predicate).AsNoTracking().ToListAsync();
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);
        public async Task SaveChangesAsync() => await unitOfWork.Context.SaveChangesAsync();
        public async Task<T> InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }
        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) throw new ArgumentException("Entity not found");
            await DeleteAsync(entity);
        }
        public async Task DeleteAsync(T entity)
        {
            if (unitOfWork.Context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }
    }
}
