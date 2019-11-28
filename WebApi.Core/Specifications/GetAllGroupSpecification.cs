using WebApi.Core.Domain.Entities;

namespace WebApi.Core.Specifications
{
    public sealed class GetAllGroupSpecification : BaseSpecification<Group>
    {
        public GetAllGroupSpecification() : base(x=>true)
        {
            AddInclude(u => u.PlayerGroupMaps);
        }
    }

    public sealed class GetAllMessageHistorySpecification : BaseSpecification<MessageHistory>
    {
        public GetAllMessageHistorySpecification(int groupId,int playerId) : base(x => x.PlayerId == playerId && x.GroupId == groupId)
        {
           
        }
    }
}