using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Dto;
using WebApi.Core.Interfaces.Repositories;
using WebApi.Core.Interfaces.Services;
using WebApi.Core.Specifications;

namespace WebApi.Core.Domain.Queries
{
    public class GetMessageOfGroupQueryHandler : IRequestHandler<GetMessagesOfGroupQuery, IList<MessageDto>>
    {
        private readonly ISecurityDataProvider _securityDataProvider;
        private readonly IRepository _repository;

        public GetMessageOfGroupQueryHandler(ISecurityDataProvider securityDataProvider, IRepository repository)
        {
            _securityDataProvider = securityDataProvider;
            _repository = repository;
        }

        public async Task<IList<MessageDto>> Handle(GetMessagesOfGroupQuery request, CancellationToken cancellationToken)
        {
            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var memberShip =await _repository.GetSingleBySpec(new GetGroupMemberShipSpecification(player.Id, request.GroupId));
            if (memberShip == null)
            {
                throw new InvalidOperationException("Can not retrieve messages of a group which the player has not joined yet");
            }
            var allMessageHistory =
                (await _repository.List(new GetAllMessageHistorySpecification(request.GroupId))).Select(x=>new MessageDto
                {
                    PlayerId = x.PlayerId,
                    PlayerName = x.Player.UserName,
                    GroupId = x.GroupId,
                    Message = x.Message,
                    SentOn = x.Created
                }).OrderBy(x=>x.SentOn).ToList();
            return allMessageHistory;
        }
    }
}