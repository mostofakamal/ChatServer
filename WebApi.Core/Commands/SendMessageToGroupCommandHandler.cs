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
    public class SendMessageToGroupCommandHandler : IRequestHandler<SendMessageToGroupCommand>
    {
        private readonly IRepository<MessageHistory> _messageHistory;
        private readonly IRepository<PlayerGroupMapping> _groupMappingRepository;
        private readonly ISecurityDataProvider _securityDataProvider;
        public SendMessageToGroupCommandHandler(IRepository<MessageHistory> messageHistory, IRepository<PlayerGroupMapping> groupMappingRepository,ISecurityDataProvider securityDataProvider)
        {
            _messageHistory = messageHistory;
            _groupMappingRepository = groupMappingRepository;
            _securityDataProvider = securityDataProvider;
        }
        public async Task<Unit> Handle(SendMessageToGroupCommand request, CancellationToken cancellationToken)
        {
            var player =await _securityDataProvider.GetCurrentLoggedInPlayer();
            var groupMember =
                await _groupMappingRepository.GetSingleBySpec(
                    new GetGroupMemberShipSpecification(player.Id, request.GroupId));
            if (groupMember == null)
            {
                throw new InvalidOperationException("Can not send message to a group which the player has not joined yet");
            }

            var messageHistory = new MessageHistory()
            {
                PlayerId = player.Id,
                GroupId = groupMember.GroupId,
                Message = request.Message,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            await _messageHistory.Add(messageHistory);
            return Unit.Value;
        }
    }
}