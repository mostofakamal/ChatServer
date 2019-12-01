using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Dto;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using WebApi.Core.Specifications;

namespace WebApi.Core.Commands
{
    public class SendMessageToGroupCommandHandler : IRequestHandler<SendMessageToGroupCommand,MessageDto>
    {
        private readonly IRepository _repository;
        private readonly ISecurityDataProvider _securityDataProvider;
        public SendMessageToGroupCommandHandler(IRepository repository, ISecurityDataProvider securityDataProvider)
        {
            _repository = repository;
            _securityDataProvider = securityDataProvider;
        }
        public async Task<MessageDto> Handle(SendMessageToGroupCommand request, CancellationToken cancellationToken)
        {
            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var groupMember =
                await _repository.GetSingleBySpec(
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
            await _repository.Add(messageHistory);
            return new MessageDto()
            {
                PlayerId = messageHistory.PlayerId,
                PlayerName = player.UserName,
                GroupId = messageHistory.GroupId,
                Message = request.Message,
                SentOn = messageHistory.Created
            };
        }
    }
}