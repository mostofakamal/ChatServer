using System;
using System.Linq;
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
    public class GetMessageOfGroupQueryHandler : IRequestHandler<GetMessagesOfGroupQuery, GetMessagesOfGroupResult>
    {
        private readonly ISecurityDataProvider _securityDataProvider;
        private readonly IRepository<MessageHistory> _messsageHistory;
        private readonly IRepository<PlayerGroupMapping> _playerGroupMappingRepo;

        public GetMessageOfGroupQueryHandler(ISecurityDataProvider securityDataProvider, IRepository<MessageHistory> messsageHistory, IRepository<PlayerGroupMapping> playerGroupMappingRepo)
        {
            _securityDataProvider = securityDataProvider;
            _messsageHistory = messsageHistory;
            _playerGroupMappingRepo = playerGroupMappingRepo;
        }

        public async Task<GetMessagesOfGroupResult> Handle(GetMessagesOfGroupQuery request, CancellationToken cancellationToken)
        {
            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var memberShip =await _playerGroupMappingRepo.GetSingleBySpec(new GetGroupMemberShipSpecification(player.Id, request.GroupId));
            if (memberShip == null)
            {
                throw new InvalidOperationException("Can not retrieve messages of a group which the player has not joined yet");
            }
            var allMessageHistory =
                (await _messsageHistory.List(new GetAllMessageHistorySpecification(request.GroupId))).Select(x=>new MessageDto
                {
                    PlayerId = x.PlayerId,
                    GroupId = x.GroupId,
                    Message = x.Message,
                    SentOn = x.Created
                }).OrderByDescending(x=>x.SentOn).ToList();
            return new GetMessagesOfGroupResult(allMessageHistory);
        }
    }
}