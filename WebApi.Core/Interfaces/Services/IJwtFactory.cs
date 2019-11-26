using System.Threading.Tasks;
using WebApi.Core.Dto;

namespace WebApi.Core.Interfaces.Services
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}
