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
        private readonly IPlayerRepository _playerRepository;

        public GetMessageOfGroupQueryHandler(ISecurityDataProvider securityDataProvider, IRepository<MessageHistory> messsageHistory, IPlayerRepository playerRepository)
        {
            _securityDataProvider = securityDataProvider;
            _messsageHistory = messsageHistory;
            _playerRepository = playerRepository;
        }

        public async Task<GetMessagesOfGroupResult> Handle(GetMessagesOfGroupQuery request, CancellationToken cancellationToken)
        {
            var userName =  _securityDataProvider.GetCurrentUserName();
            var player = await _playerRepository.FindByName(userName);
            var allMessageHistory =
                (await _messsageHistory.List(new GetAllMessageHistorySpecification(request.GroupId, player.Id))).Select(x=>new MessageDto
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