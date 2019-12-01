using MediatR;
using WebApi.Core.Dto;

namespace WebApi.Core.Commands
{
    public class SendMessageToGroupCommand : IRequest<MessageDto>
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