using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Interfaces.Repositories;

namespace WebApi.Infrastructure.Data.Repositories
{

    public abstract class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly GameDbContext GameDbContext;

        protected EfRepository(GameDbContext gameDbContext)
        {
            GameDbContext = gameDbContext;
        }

        public virtual async Task<T> GetById(int id)
        {
            return await GameDbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAll()
        {
            return await GameDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetSingleBySpec(ISpecification<T> spec)
        {
            var result = await List(spec);
            return result.FirstOrDefault();
        }

        public async Task<List<T>> List(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(GameDbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                            .Where(spec.Criteria)
                            .ToListAsync();
        }


        public async Task<T> Add(T entity)
        {
            GameDbContext.Set<T>().Add(entity);
            await GameDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            GameDbContext.Entry(entity).State = EntityState.Modified;
            await GameDbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            GameDbContext.Set<T>().Remove(entity);
            await GameDbContext.SaveChangesAsync();
        }
    }
}
