using System.Threading.Tasks;
using WebApi.Core.Domain.Entities;

namespace WebApi.Core.Interfaces.Repositories
{
    public interface IPlayerRepository  : IRepository<Player>
    {
        Task<int> Create(string firstName, string lastName, string email, string userName, string password);
        Task<Player> FindByName(string userName);
        Task<bool> CheckPassword(Player user, string password);
    }
}
