using Microsoft.EntityFrameworkCore;
using Pastebook.Data.Models;
using Pastebook.Web.Data;
using Pastebook.Web.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pastebook.Web.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> FindAll();
        Task<T> FindByPrimaryKey(Guid id);
        Task<T> Insert(T entity);
        T Update(T entity);
        Task<T> Delete(Guid id);
        T Delete(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        Task<IEnumerable<T>> FindQuery(string query, params object[] parameters);
    }

    public abstract class GenericRepository<T> where T : BaseEntity
    {
        public GenericRepository(PastebookContext context)
        {
            this.Context = context;
        }

        public PastebookContext Context { get; set; }

        public async Task<IEnumerable<T>> FindAll()
        {
            IQueryable<T> query = this.Context.Set<T>();
            return await query.Select(e => e).ToListAsync();
        }

        public async Task<T> FindByPrimaryKey(Guid id)
        {
            var entity = await this.Context.FindAsync<T>(id);
            if (entity is object)
            {
                return entity;
            }

            throw new Exception($"Entity does not exist {id}");
        }

        public async Task<T> Insert(T entity)
        {
            if (entity is null)
            {
                throw new EntityDataException("Entity argument cannot be null.");
            }

            await Context.AddAsync<T>(entity);

            return entity;
        }

        public T Update(T entity)
        {
            if (entity is null)
            {
                throw new EntityDataException("Entity argument cannot be null.");
            }

            this.Context.Attach<T>(entity);
            this.Context.Entry<T>(entity).State = EntityState.Modified;

            return entity;
        }

        public async Task<T> Delete(Guid id)
        {
            var entity = await this.Context.FindAsync<T>(id);
            if (entity is object)
            {
                this.Context.Remove<T>(entity);
                return entity;
            }

            throw new EntityDataException("Entity not found.");
        }

        public T Delete(T entity)
        {
            if (entity is null)
            {
                throw new EntityDataException("Entity argument cannot be null.");
            }

            if (this.Context.Entry(entity).State == EntityState.Detached)
            {
                this.Context.Set<T>().Attach(entity);
            }

            this.Context.Set<T>().Remove(entity);

            return entity;
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = this.Context.Set<T>();

            if (filter is object)
            {
                query = query.Where(filter);
            }

            if (includeProperties is object)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy is object)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindQuery(string sqlQuery, params object[] parameters)
        {
            var query = this.Context.Set<T>();
            return await query.FromSqlRaw(sqlQuery, parameters).ToListAsync();
        }
    }
}
