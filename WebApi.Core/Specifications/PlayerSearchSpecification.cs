using System;
using System.Linq.Expressions;
using WebApi.Core.Domain.Entities;

namespace WebApi.Core.Specifications
{
    public sealed class PlayerSearchSpecification : BaseSpecification<Player>
    {
        public PlayerSearchSpecification(string identityId) : base(u => u.IdentityId==identityId)
        {
            AddInclude(u => u.RefreshTokens);
        }
    }
}
