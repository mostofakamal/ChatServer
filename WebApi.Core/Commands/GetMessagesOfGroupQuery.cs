using System.Collections.Generic;
using MediatR;
using WebApi.Core.Dto;

namespace WebApi.Core.Commands
{
    public class GetMessagesOfGroupQuery : IRequest<IList<MessageDto>>
    {
        public GetMessagesOfGroupQuery(int groupId)
        {
            GroupId = groupId;
        }

        public int GroupId { get; }
    }
}