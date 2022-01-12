using KnowledgeBet.Core.Interfaces;
using KnowledgeBet.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBet.API.Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
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

        [MapToApiVersion("1.0")]
        [HttpGet("", Name = "GetAllPlayers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllPlayers()
        {
            var players = knowledgeBetService.GetAllPlayers();
            return Ok(players);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("", Name = "GetAllPlayedGames")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllPlayedGames()
        {
            var playedGames = knowledgeBetService.GetAllPlayedGames();
            return Ok(playedGames);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("", Name = "CreateNewQuestion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult CreateNewQuestion()
        {
            return Ok();
        }
    }
}
