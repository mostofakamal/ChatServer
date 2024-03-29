﻿using System;

namespace WebApi.Core.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; }
        public DateTime Expires { get; private set; }
        public int PlayerId { get; private set; }
        public bool Active => DateTime.UtcNow <= Expires;
        public string RemoteIpAddress { get; private set; }

        public RefreshToken(string token, DateTime expires, int playerId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            PlayerId = playerId;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}