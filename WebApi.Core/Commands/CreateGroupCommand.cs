using MediatR;

namespace WebApi.Core.Commands
{
    public class CreateGroupCommand : IRequest<CreateGroupResponse>
    {
        public string Name { get; set; }
    }
}