using Microsoft.EntityFrameworkCore;
using Pastebook.Data.Data;
using Pastebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> FindAll();
        Task<T> FindByPrimaryKey(Guid id);
        T Insert(T entity);
        T Update(T entity);
        Task<T> Delete(Guid id);
    }

    public class GenericRepository<T> where T : BaseEntity
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
                this.Context.Entry<T>(entity).State = EntityState.Detached;
                return entity;
            }

            throw new Exception($"Entity does not exist {id}");
        }

        public T Insert(T entity)
        {
            Context.Add<T>(entity);
            Context.SaveChanges();
            return entity;
        }


        public T Update(T entity)
        {

            this.Context.Attach<T>(entity);


            this.Context.Entry<T>(entity).State = EntityState.Modified;
            this.Context.SaveChanges();
            return entity;
        }

        public async Task<T> Delete(Guid id)
        {
            var entity = await this.FindByPrimaryKey(id);
            this.Context.Remove<T>(entity);
            this.Context.SaveChanges();
            return entity;
        }
    }
}
