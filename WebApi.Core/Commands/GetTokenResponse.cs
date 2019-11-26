using WebApi.Core.Dto;

namespace WebApi.Core.Commands
{
    public class GetTokenResponse
    {
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}