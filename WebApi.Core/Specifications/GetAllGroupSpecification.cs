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
        public GetAllMessageHistorySpecification(int groupId) : base(x=> x.GroupId == groupId)
        {
           AddInclude(x=>x.Player);
        }
    }
}