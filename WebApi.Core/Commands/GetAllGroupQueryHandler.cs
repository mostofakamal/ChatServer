using System.Collections.Generic;
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
    public class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, IList<GroupDto>>
    {
        private readonly IRepository _repository;
        private readonly ISecurityDataProvider _securityDataProvider;
        public GetAllGroupQueryHandler(IRepository repository,
            ISecurityDataProvider securityDataProvider)
        {
            _repository = repository;
            _securityDataProvider = securityDataProvider;
        }
        public async Task<IList<GroupDto>> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
        {
            var groups = await _repository.List(new GetAllGroupSpecification());
            var player = await _securityDataProvider.GetCurrentLoggedInPlayer();
            var result = groups?.Select(x => new GroupDto
            {
                Name = x.Name,
                GroupId = x.Id,
                IsMember =  x.PlayerGroupMaps.Any(y=>y.PlayerId == player.Id)
            }).ToList();
            return result;
        }
    }
}