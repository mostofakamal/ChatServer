using System.Threading.Tasks;
using ChatServer.Dtos;
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

    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediatR;

        /// <summary>
        /// The Message controller
        /// </summary>
        /// <param name="mediatR"></param>
        public MessagesController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        /// <summary>
        /// Sends message to a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("groups/{groupId}")]
        public async Task<IActionResult> SendMessage(int groupId,SendMessageDto request)
        {
            return Ok(await _mediatR.Send(new SendMessageToGroupCommand(groupId, request.Message)));
        }

        /// <summary>
        /// Gets all message of a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("groups/{groupId}")]
        public async Task<IActionResult> GetAllMessageHistory(int groupId)
        {
            return Ok(await _mediatR.Send(new GetMessagesOfGroupQuery(groupId)));
        }

    }
}