using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;

namespace WebApi.Core.Commands
{
    public class GetTokenCommandHandler : IRequestHandler<GetTokenCommand,GetTokenResponse>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;
        public GetTokenCommandHandler(IPlayerRepository playerRepository, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _playerRepository = playerRepository;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }
        public async Task<GetTokenResponse> Handle(GetTokenCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Password))
            {
                // ensure we have a user with the given user name
                var player = await _playerRepository.FindByName(request.UserName);
                if (player != null)
                {
                    // validate password
                    if (await _playerRepository.CheckPassword(player, request.Password))
                    {
                        // generate refresh token
                        var refreshToken = _tokenFactory.GenerateToken();
                        player.AddRefreshToken(refreshToken, player.Id, request.RemoteIpAddress);
                        await _playerRepository.Update(player);

                        // generate access token
                        var generateEncodedToken = await _jwtFactory.GenerateEncodedToken(player.IdentityId, player.UserName);
                        return new GetTokenResponse{
                            AccessToken = generateEncodedToken,
                            RefreshToken = refreshToken

                        };
                    }
                }
            }

            return null;
        }
    }
}