using WebApi.Core.Domain.Entities;

namespace WebApi.Core.Specifications
{
    public sealed class GroupSearchSpecification : BaseSpecification<Group>
    {
        public GroupSearchSpecification(int id) : base(x=>x.Id== id)
        {
            AddInclude(u=>u.PlayerGroupMaps);
        }
    }
}