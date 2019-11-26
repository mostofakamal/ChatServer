using MediatR;

namespace WebApi.Core.Commands
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