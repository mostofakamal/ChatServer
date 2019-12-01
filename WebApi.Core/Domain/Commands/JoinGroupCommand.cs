using MediatR;

namespace WebApi.Core.Domain.Commands
{
    public class JoinGroupCommand : IRequest
    {

        public JoinGroupCommand(int groupId)
        {
            GroupId = groupId;
        }

        public int GroupId { get; }
    }
}