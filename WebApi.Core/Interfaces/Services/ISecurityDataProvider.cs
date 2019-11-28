using System.Threading.Tasks;
using WebApi.Core.Domain.Entities;

namespace WebApi.Core.Interfaces.Services
{
    public interface ISecurityDataProvider
    {
        string GetCurrentUserName();
        Task<Player> GetCurrentLoggedInPlayer();
    }
}
