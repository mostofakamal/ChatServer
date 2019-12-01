using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Domain.Commands;
using WebApi.Core.Domain.Queries;

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



        /// <summary>
        /// Gets all groups
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediatR.Send(new GetAllGroupQuery()));
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

        // POST api/groups
        /// <summary>
        /// Leaves a group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("groups/{id}/player")]
        public async Task<IActionResult> LeaveGroup(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediatR.Send(new LeaveGroupCommand(id));
            return Ok(result);
        }

    }
}