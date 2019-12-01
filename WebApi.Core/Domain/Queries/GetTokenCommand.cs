using MediatR;

namespace WebApi.Core.Domain.Queries
{
    public class GetTokenCommand : IRequest<GetTokenResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string RemoteIpAddress { get; set; }
    }
}