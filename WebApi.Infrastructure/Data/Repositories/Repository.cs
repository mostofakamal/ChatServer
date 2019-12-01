using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Interfaces.Repositories;

namespace WebApi.Infrastructure.Data.Repositories
{

    public class Repository : IRepository
    {
        protected readonly GameDbContext GameDbContext;

        public Repository(GameDbContext gameDbContext)
        {
            GameDbContext = gameDbContext;
        }

        public virtual async Task<T> GetById<T>(int id) where T : BaseEntity
        {
            return await GameDbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAll<T>() where T : BaseEntity
        {
            return await GameDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetSingleBySpec<T>(ISpecification<T> spec) where T : BaseEntity
        {
            var result = await List(spec);
            return result.FirstOrDefault();
        }

        public async Task<List<T>> List<T>(ISpecification<T> spec) where T : BaseEntity
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


        public async Task<T> Add<T>(T entity) where T : BaseEntity
        {
            GameDbContext.Set<T>().Add(entity);
            await GameDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Update<T>(T entity) where T : BaseEntity
        {
            GameDbContext.Entry(entity).State = EntityState.Modified;
            await GameDbContext.SaveChangesAsync();
        }

        public async Task Delete<T>(T entity) where T : BaseEntity
        {
            GameDbContext.Set<T>().Remove(entity);
            await GameDbContext.SaveChangesAsync();
        }
    }
}
