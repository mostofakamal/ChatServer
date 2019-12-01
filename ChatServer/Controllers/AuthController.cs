using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core.Domain.Commands;
using WebApi.Core.Domain.Queries;

namespace ChatServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/auth/login
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(GetTokenCommand request)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var result = await _mediator.Send(request);
            if (result != null)
            {
                return Ok(result);
            }

            return Unauthorized(new InvalidLoginResponse());
        }
    }
}
