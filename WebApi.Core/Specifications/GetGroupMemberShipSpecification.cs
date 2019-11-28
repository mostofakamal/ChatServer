using WebApi.Core.Domain.Entities;

namespace WebApi.Core.Specifications
{
    public sealed class GetGroupMemberShipSpecification : BaseSpecification<PlayerGroupMapping>
    {
        public GetGroupMemberShipSpecification(int playerId,int groupId) : base(x =>x.PlayerId == playerId && groupId == x.GroupId)
        {
        }
    }
}