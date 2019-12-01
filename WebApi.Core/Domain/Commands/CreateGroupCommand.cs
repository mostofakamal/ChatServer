using MediatR;

namespace WebApi.Core.Domain.Commands
{
    public class CreateGroupCommand : IRequest<CreateGroupResponse>
    {
        public string Name { get; set; }
    }
}