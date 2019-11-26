using System.Collections.Generic;

namespace WebApi.Core.Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<PlayerGroupMapping> PlayerGroupMaps { get; set; }
    }
}