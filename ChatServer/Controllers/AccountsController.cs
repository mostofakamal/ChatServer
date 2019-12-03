using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Domain.Commands;

namespace ChatServer.Controllers
{
    /// <summary>
    /// The Account Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/accounts
        /// <summary>
        /// Creates Player user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreatePlayerCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result=  await _mediator.Send(request);
            return Ok(result);
        }
    }
}
