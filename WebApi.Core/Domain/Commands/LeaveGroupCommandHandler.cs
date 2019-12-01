using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using WebApi.Core.Specifications;

namespace WebApi.Core.Domain.Commands
{
    public class LeaveGroupCommandHandler : IRequestHandler<LeaveGroupCommand, Unit>
    {

        private readonly ISecurityDataProvider _securityDataProvider;
        private readonly IRepository _repository;

        public LeaveGroupCommandHandler(IRepository repository, ISecurityDataProvider securityDataProvider)
        {
            _repository = repository;
            _securityDataProvider = securityDataProvider;
        }

        public async Task<Unit> Handle(LeaveGroupCommand request, CancellationToken cancellationToken)
        {

            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var group = await _repository.GetSingleBySpec(new GroupSearchSpecification(request.GroupId));
            var groupMember = group.PlayerGroupMaps.SingleOrDefault(x => x.PlayerId == player.Id);
            if (groupMember == null)
            {
                throw new InvalidOperationException("Player is not member of the group");
            }

            await _repository.Delete(groupMember);
            return Unit.Value;
        }
    }
}