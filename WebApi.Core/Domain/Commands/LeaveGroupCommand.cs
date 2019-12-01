using MediatR;

namespace WebApi.Core.Domain.Commands
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