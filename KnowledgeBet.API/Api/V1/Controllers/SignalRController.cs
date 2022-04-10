using KnowledgeBet.API.Api.V1.Models.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KnowledgeBet.API.Api.V1.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/sockets")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private readonly IHubContext<AppHub> _hub;

        public SignalRController(IHubContext<AppHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("send-message-to-frontend")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult> SendMessage([FromBody] Message message)
        {
            await _hub.Clients.All.SendAsync("transferTilesData", message.User + message.Text);
            return this.Ok(new { Message = "Request Completed" });
        }
    }
}
