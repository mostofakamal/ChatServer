using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebApi.Core.Domain.Entities;
using WebApi.Core.Interfaces.Repositories;

namespace WebApi.Core.Commands
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, CreateGroupResponse>
    {
        private readonly IRepository<Group> _repository;

        public CreateGroupCommandHandler(IRepository<Group> repository)
        {
            _repository = repository;
        }
        public async Task<CreateGroupResponse> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = new Group()
            {
                Name = request.Name,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };
            await _repository.Add(group);
            return new CreateGroupResponse
            {
                GroupId = group.Id
            };
        }
    }
}