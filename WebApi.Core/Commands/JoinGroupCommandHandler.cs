using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using WebApi.Core.Specifications;

namespace WebApi.Core.Commands
{
    public class JoinGroupCommandHandler : IRequestHandler<JoinGroupCommand, Unit>
    {
        private readonly ISecurityDataProvider _securityDataProvider;
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
     


        public JoinGroupCommandHandler(IRepository repository, ISecurityDataProvider securityDataProvider, IConfiguration configuration)
        {
            _repository = repository;
            _securityDataProvider = securityDataProvider;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
        {
            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var group = await _repository.GetSingleBySpec(new GroupSearchSpecification(request.GroupId));
            if (group.PlayerGroupMaps.Any(x => x.PlayerId == player.Id))
            {
                throw new InvalidOperationException("Player already joined the group");
            }

            int maxAllowedGroupMemberCount = int.Parse(_configuration["MaxAllowedGroupMemberCount"]);

            if (group.PlayerGroupMaps.Count < maxAllowedGroupMemberCount)
            {
                throw new InvalidOperationException($"Max group member limit exceeded. Allowed Limit: "+ maxAllowedGroupMemberCount);
            }
            var playerGroupMapping = new PlayerGroupMapping
            {
                PlayerId = player.Id,
                GroupId = group.Id,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            await _repository.Add(playerGroupMapping);
            return Unit.Value;
        }
    }
}