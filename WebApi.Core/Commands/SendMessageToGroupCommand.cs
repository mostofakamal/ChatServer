using MediatR;

namespace WebApi.Core.Commands
{
    public class SendMessageToGroupCommand : IRequest
    {
        public SendMessageToGroupCommand(int groupId,string message)
        {
            GroupId = groupId;
            Message = message;
        }

        public int GroupId { get; }

        public string Message { get; }
    }
}