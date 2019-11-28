﻿namespace WebApi.Core.Domain.Entities
{
    public class PlayerGroupMapping : BaseEntity
    {
        public int PlayerId { get; set; }

        public int GroupId { get; set; }

        public Player Player { get; set; }

        public Group Group { get; set; }

    }
}