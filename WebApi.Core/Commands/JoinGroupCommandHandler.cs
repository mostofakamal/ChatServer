using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Dto;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using WebApi.Core.Specifications;

namespace WebApi.Core.Commands
{
    public class JoinGroupCommandHandler : IRequestHandler<JoinGroupCommand, Unit>
    {
        private readonly IJwtTokenValidator _tokenValidator;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AuthSettings _authSettings;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<PlayerGroupMapping> _playerGroupMappingRepository;


        public JoinGroupCommandHandler(IJwtTokenValidator tokenValidator, IHttpContextAccessor httpContextAccessor, IOptions<AuthSettings> authOptions, IPlayerRepository playerRepository, IRepository<Group> groupRepository, IRepository<PlayerGroupMapping> playerGroupMappingRepository)
        {
            _tokenValidator = tokenValidator;
            _httpContextAccessor = httpContextAccessor;
            _playerRepository = playerRepository;
            _groupRepository = groupRepository;
            _playerGroupMappingRepository = playerGroupMappingRepository;
            _authSettings = authOptions.Value;
        }

        public async Task<Unit> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
        {
            //TODO: Refactor this out to a seperate class e.g Security Data Provider
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ",""); ;

            var claimsPrincipal = _tokenValidator.GetPrincipalFromToken(accessToken, _authSettings.SecretKey);
            
            var userName = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault();
            var player = await _playerRepository.FindByName(userName);
            var group = await _groupRepository.GetSingleBySpec(new GroupSearchSpecification(request.GroupId));
            var playerGroupMapping = new PlayerGroupMapping
            {
                UserId = player.Id,
                GroupId = group.Id,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            await _playerGroupMappingRepository.Add(playerGroupMapping);
            return Unit.Value;
        }
    }
}