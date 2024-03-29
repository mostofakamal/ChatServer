﻿using MediatR;

namespace WebApi.Core.Domain.Commands
{
    /// <summary>
    /// Player create command
    /// </summary>
    public class CreatePlayerCommand : IRequest<CreatePlayerResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
