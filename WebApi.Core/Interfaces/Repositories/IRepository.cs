using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Core.Domain.Entities;

namespace WebApi.Core.Interfaces.Repositories
{
    public interface IRepository 
    {
        Task<T> GetById<T>(int id) where T : BaseEntity;
        Task<List<T>> ListAll<T>() where T : BaseEntity;
        Task<T> GetSingleBySpec<T>(ISpecification<T> spec) where T : BaseEntity;
        Task<List<T>> List<T>(ISpecification<T> spec) where T : BaseEntity;
        Task<T> Add<T>(T entity) where T : BaseEntity;
        Task Update<T>(T entity) where T : BaseEntity;
        Task Delete<T>(T entity) where T : BaseEntity;

    }
}
