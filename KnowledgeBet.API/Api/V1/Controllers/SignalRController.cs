using KnowledgeBet.API.HubConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KnowledgeBet.API.Api.V1.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private IHubContext<ChartHub> _hub;

        public SignalRController(IHubContext<ChartHub> hub)
        {
            _hub = hub;
        }

        [HttpGet("", Name = "GetSignalR")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetSignalR()
        {
            _hub.Clients.All.SendAsync("transferData", "Server Message");

            return Ok(new { Message = "Request Completed" });
        }
    }
}
