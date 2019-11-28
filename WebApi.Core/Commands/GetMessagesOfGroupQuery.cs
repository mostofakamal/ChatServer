using MediatR;

namespace WebApi.Core.Commands
{
    public class GetMessagesOfGroupQuery : IRequest<GetMessagesOfGroupResult>
    {
        public GetMessagesOfGroupQuery(int groupId)
        {
            GroupId = groupId;
        }

        public int GroupId { get; }
    }
}