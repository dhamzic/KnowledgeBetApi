using KnowledgeBet.Core.Interfaces;
using KnowledgeBet.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBet.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
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

        [HttpGet("", Name = "GetAllPlayers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllPlayers()
        {
            var players = knowledgeBetService.GetAllPlayers();
            return Ok(players);
        }

        [HttpGet("", Name = "GetAllPlayedGames")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllPlayedGames()
        {
            var playedGames = knowledgeBetService.GetAllPlayedGames();
            return Ok(playedGames);
        }

        [HttpPost("", Name = "CreateNewQuestion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult CreateNewQuestion()
        {
            return Ok();
        }
    }
}
