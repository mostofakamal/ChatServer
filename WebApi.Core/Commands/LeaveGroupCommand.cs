using MediatR;

namespace WebApi.Core.Commands
{
    public class LeaveGroupCommand : IRequest
    {

        public LeaveGroupCommand(int groupId)
        {
            GroupId = groupId;
        }

        public int GroupId { get; }
    }
}