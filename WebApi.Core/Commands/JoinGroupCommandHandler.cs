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
        private readonly ISecurityDataProvider _securityDataProvider;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<PlayerGroupMapping> _playerGroupMappingRepository;


        public JoinGroupCommandHandler(IRepository<Group> groupRepository, IRepository<PlayerGroupMapping> playerGroupMappingRepository, ISecurityDataProvider securityDataProvider)
        {
            _groupRepository = groupRepository;
            _playerGroupMappingRepository = playerGroupMappingRepository;
            _securityDataProvider = securityDataProvider;
        }

        public async Task<Unit> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
        {
            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var group = await _groupRepository.GetSingleBySpec(new GroupSearchSpecification(request.GroupId));
            if (group.PlayerGroupMaps.Any(x => x.PlayerId == player.Id))
            {
                throw new InvalidOperationException("Player already joined the group");
            }
            var playerGroupMapping = new PlayerGroupMapping
            {
                PlayerId = player.Id,
                GroupId = group.Id,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            await _playerGroupMappingRepository.Add(playerGroupMapping);
            return Unit.Value;
        }
    }
}