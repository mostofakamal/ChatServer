using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebApi.Core.Dto;
using WebApi.Core.Interfaces.Services;

namespace WebApi.Infrastructure.Auth
{
    public class SecurityDataProvider: ISecurityDataProvider
    {
        private readonly IJwtTokenValidator _tokenValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthSettings _authSettings;

        public SecurityDataProvider(IJwtTokenValidator tokenValidator, 
            IHttpContextAccessor httpContextAccessor,
            IOptions<AuthSettings> authSettings)
        {
            _tokenValidator = tokenValidator;
            _httpContextAccessor = httpContextAccessor;
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
    }
}
