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
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, GetAllGroupQueryResult>
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly ISecurityDataProvider _securityDataProvider;
        public GetAllGroupQueryHandler(IRepository<Group> groupRepository,
            ISecurityDataProvider securityDataProvider)
        {
            _groupRepository = groupRepository;
            _securityDataProvider = securityDataProvider;
        }
        public async Task<GetAllGroupQueryResult> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.List(new GetAllGroupSpecification());
            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var result = groups?.Select(x => new GroupDto
            {
                Name = x.Name,
                GroupId = x.Id,
                IsMember =  x.PlayerGroupMaps.Any(y=>y.PlayerId == player.Id)
            }).ToList();
            return new GetAllGroupQueryResult(result);
        }
    }
}