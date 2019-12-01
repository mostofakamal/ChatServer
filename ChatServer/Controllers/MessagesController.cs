using System.Threading.Tasks;
using ChatServer.Dtos;
using ChatServer.HubConstants;
using ChatServer.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<MessageHub> _messageHubContext;

        /// <summary>
        /// The Message controller
        /// </summary>
        /// <param name="mediatR"></param>
        public MessagesController(IMediator mediatR, IHubContext<MessageHub> messageHubContext)
        {
            _mediatR = mediatR;
            _messageHubContext = messageHubContext;
        }

        /// <summary>
        /// Sends message to a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("groups/{groupId:int}")]
        public async Task<IActionResult> SendMessage(int groupId,SendMessageDto request)
        {
            var sendMessageToGroupResult = await _mediatR.Send(new SendMessageToGroupCommand(groupId, request.Message));
            await _messageHubContext.Clients.Groups(sendMessageToGroupResult.GroupId.ToString()).SendAsync(HubMethodNames.MessageReceived, sendMessageToGroupResult);
            return Ok(sendMessageToGroupResult);
        }



        /// <summary>
        /// Gets all message of a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("groups/{groupId:int}")]
        public async Task<IActionResult> GetAllMessageHistory(int groupId)
        {
            var messages = await _mediatR.Send(new GetMessagesOfGroupQuery(groupId));
            return Ok(messages);
        }

    }
}