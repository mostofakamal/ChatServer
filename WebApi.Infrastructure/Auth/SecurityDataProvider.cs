using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Dto;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;

namespace WebApi.Infrastructure.Auth
{
    public class SecurityDataProvider: ISecurityDataProvider
    {
        private readonly IJwtTokenValidator _tokenValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthSettings _authSettings;
        private readonly IPlayerRepository _playerRepository;

        public SecurityDataProvider(IJwtTokenValidator tokenValidator, 
            IHttpContextAccessor httpContextAccessor,
            IOptions<AuthSettings> authSettings, IPlayerRepository playerRepository)
        {
            _tokenValidator = tokenValidator;
            _httpContextAccessor = httpContextAccessor;
            _playerRepository = playerRepository;
            _authSettings = authSettings.Value;
        }
        public string GetCurrentUserName()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""); ;
            var claimsPrincipal = _tokenValidator.GetPrincipalFromToken(accessToken, _authSettings.SecretKey);

            var userName = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault();
            return userName;
        }

        public async Task<Player> GetCurrentLoggedInPlayer()
        {
            var userName = GetCurrentUserName();
            return await _playerRepository.FindByName(userName);
        }
    }
}
