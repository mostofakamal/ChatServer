using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using WebApi.Core.Specifications;

namespace WebApi.Core.Commands
{
    public class LeaveGroupCommandHandler : IRequestHandler<LeaveGroupCommand, Unit>
    {

        private readonly ISecurityDataProvider _securityDataProvider;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<PlayerGroupMapping> _playerGroupMappingRepository;

        public LeaveGroupCommandHandler(IRepository<Group> groupRepository, IRepository<PlayerGroupMapping> playerGroupMappingRepository, ISecurityDataProvider securityDataProvider)
        {
            _groupRepository = groupRepository;
            _playerGroupMappingRepository = playerGroupMappingRepository;
            _securityDataProvider = securityDataProvider;
        }

        public async Task<Unit> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
        {

            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var group = await _groupRepository.GetSingleBySpec(new GroupSearchSpecification(request.GroupId));
            var groupMember = group.PlayerGroupMaps.SingleOrDefault(x => x.PlayerId == player.Id);
            if (groupMember == null)
            {
                throw new InvalidOperationException("Player is not member of the group");
            }

            await _playerGroupMappingRepository.Delete(groupMember);
            return Unit.Value;
        }
    }
}