using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Core.Domain.Entities
{
    public class Player : BaseEntity
    {
        public string FirstName { get; private set; } 
        public string LastName { get; private set; }
        public string IdentityId { get; private set; }
        public string UserName { get; private set; } 
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public ICollection<PlayerGroupMapping> PlayerGroupMaps { get; set; }

        internal Player() { }

        public Player(string firstName, string lastName, string identityId, string userName)
        {
            FirstName = firstName;
            LastName = lastName;
            IdentityId = identityId;
            UserName = userName;
        }

        public bool HasValidRefreshToken(string refreshToken)
        {
            return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token, int playerId, string remoteIpAddress, double daysToExpire = 5)
        {
            _refreshTokens.Add(new RefreshToken(token, DateTime.UtcNow.AddDays(daysToExpire), playerId, remoteIpAddress));
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            _refreshTokens.Remove(_refreshTokens.First(t => t.Token == refreshToken));
        }
    }
}
