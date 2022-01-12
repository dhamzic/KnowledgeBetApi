using KnowledgeBet.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBet.API.Api.V2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[action]")]
    [Produces("application/json")]
    public class KnowledgeBetController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IKnowledgeBetService knowledgeBetService;

        public KnowledgeBetController(
            ILogger<KnowledgeBetController> logger,
            IKnowledgeBetService knowledgeBetService)
        {
            this.logger = logger;
            this.knowledgeBetService = knowledgeBetService;
        }

        [MapToApiVersion("2.0")]
        [HttpGet("", Name = "GetAllPlayers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllPlayers()
        {
            var players = knowledgeBetService.GetAllPlayers();
            return Ok(players);
        }
    }
}
