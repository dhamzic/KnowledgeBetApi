using Domain.Entities.Models;
using Domain.Interfaces.Repositories;
using KnowledgeBet.API.Api.V1.Models;
using KnowledgeBet.API.Api.V1.Models.Requests;
using KnowledgeBet.API.Api.V1.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBet.API.Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/games")]
    [Produces("application/json")]
    public class GamesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IGameRepository _gameRepository;

        public GamesController(ILogger<GamesController> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetGamesResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<GetGamesResponse>> GetGames()
        {
            {
                try
                {
                    _logger.LogInformation("GetGames method has been called");

                    var result = await _gameRepository.GetAllPlayedGames();

                    var retVal = new GetGamesResponse()
                    {
                        Data = result,
                        Status = ResponseStatus.Success
                    };

                    return this.Ok(retVal);

                }
                catch (Exception ex)
                {
                    return this.BadRequest(new BaseResponse()
                    {
                        Status = ResponseStatus.UnhandledException,
                        LogMessage = ex.Message
                    });
                }
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateGameResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<CreateGameResponse>> CreateGame([FromBody] CreateGameRequest createGameRequest)
        {
            try
            {
                _logger.LogInformation("CreateGame method method has been called");

                var result = await _gameRepository.CreateNewGame(createGameRequest.Date, createGameRequest.PlayersId, createGameRequest.QuestionsId, createGameRequest.WinnerId);

                var retVal = new CreateGameResponse(result)
                {
                    Status = ResponseStatus.Success
                };

                return CreatedAtAction(nameof(GetGame), new { id = retVal.Data.Id }, retVal);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new BaseResponse()
                {
                    Status = ResponseStatus.UnhandledException,
                    LogMessage = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GameModel>> GetGame(int id)
        {
            return await _gameRepository.GetGame(id);
        }
    }
}
