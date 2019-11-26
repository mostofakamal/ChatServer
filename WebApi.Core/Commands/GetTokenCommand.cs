using MediatR;

namespace WebApi.Core.Commands
{
    public class GetTokenCommand : IRequest<GetTokenResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RemoteIpAddress { get; set; }
    }
}