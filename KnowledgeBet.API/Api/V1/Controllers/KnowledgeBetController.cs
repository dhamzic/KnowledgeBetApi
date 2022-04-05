using Domain.Entities.Models;
using Domain.Interfaces.Repositories;
using KnowledgeBet.API.Api.V1.Models;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBet.API.Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[action]")]
    [Produces("application/json")]
    public class KnowledgeBetController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IKnowledgeBetService _knowledgeBetService;

        public KnowledgeBetController(
            ILogger<KnowledgeBetController> logger,
            IKnowledgeBetService knowledgeBetService)
        {
            this._logger = logger;
            this._knowledgeBetService = knowledgeBetService;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("", Name = "GetAllPlayers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllPlayers()
        {
            var players = _knowledgeBetService.GetAllPlayers();
            return Ok(players);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("", Name = "GetAllPlayedGames")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetAllPlayedGames()
        {
            var playedGames = _knowledgeBetService.GetAllPlayedGames();
            return Ok(playedGames);
        }

        [MapToApiVersion("1.0")]
        [HttpPost("", Name = "CreateNewQuestion")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateNewQuestion([FromBody] NewQuestionRequestModel newQuestionRequestModel)
        {
            try
            {
                var newQuestionDTO = new NewQuestionModel
                {
                    Text = newQuestionRequestModel.Text,
                    SubcategoryId = newQuestionRequestModel.SubcategoryId,
                    Options = newQuestionRequestModel.Options.Select(o => new QuestionOptionModel
                    {
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                };

                var createdQuestion = await _knowledgeBetService.CreateNewQuestion(newQuestionDTO);
                _logger.LogDebug("Question successfully created", newQuestionRequestModel.Text);
                return Ok(createdQuestion);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpPost("", Name = "CreateNewGame")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateNewGame([FromBody] NewGameRequestModel newGameRequestModel)
        {
            try
            {
                var newGameDto = new NewGameModel
                {
                    Date = newGameRequestModel.Date,
                    PlayersId = newGameRequestModel.PlayersId,
                    QuestionsId = newGameRequestModel.QuestionsId,
                    PlayerWinnerId = newGameRequestModel.WinnerId
                };

                var createdQuestion = await _knowledgeBetService.CreateNewGame(newGameDto);
                _logger.LogDebug("Game successfully created", createdQuestion);
                return Ok(createdQuestion);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("", Name = "DeleteQuestion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteQuestion([FromBody] int questionId)
        {
            try
            {
                var deletedQuestion = await _knowledgeBetService.DeleteQuestion(questionId);
                _logger.LogDebug("Question successfully deleted", deletedQuestion);
                return Ok(deletedQuestion);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpPut("", Name = "DeactivateQuestion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeactivateQuestion([FromBody] int questionId)
        {
            try
            {
                var deactivatedQuestion = await _knowledgeBetService.DeactivateQuestion(questionId);
                _logger.LogDebug("Question successfully deactivated", deactivatedQuestion);
                return Ok(deactivatedQuestion);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }
    }
}
