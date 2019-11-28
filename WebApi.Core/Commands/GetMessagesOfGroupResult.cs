using System.Collections.Generic;
using WebApi.Core.Dto;

namespace WebApi.Core.Commands
{
    public class GetMessagesOfGroupResult
    {
        public GetMessagesOfGroupResult(IList<MessageDto> messages)
        {
            Messages = messages;
        }

        public IList<MessageDto> Messages { get; }
    }
}