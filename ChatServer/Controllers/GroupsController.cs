﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Commands;

namespace ChatServer.Controllers
{
    /// <summary>
    /// The groups Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediatR;
        public GroupsController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        // POST api/groups
        /// <summary>
        /// Creates Group
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateGroupCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediatR.Send(request);
            return Ok(result);
        }

        // POST api/groups
        /// <summary>
        /// Joins a group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("groups/{id}/player")]
        public async Task<IActionResult> JoinGroup(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediatR.Send(new JoinGroupCommand(id));
            return Ok(result);
        }

    }
}