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
    [Route("api/v{version:apiVersion}/players")]
    [Produces("application/json")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(ILogger<PlayersController> logger, IPlayerRepository playerRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPlayersResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<GetPlayersResponse>> GetPlayers()
        {
            {
                try
                {
                    _logger.LogInformation("GetAllPlayers method has been called");

                    var result = await _playerRepository.GetAllPlayers();

                    var retVal = new GetPlayersResponse()
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreatePlayerResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<CreatePlayerResponse>> CreatePlayer([FromBody] CreatePlayerRequest player)
        {
            try
            {
                _logger.LogInformation("CreatePlayer method method has been called");

                var result = await _playerRepository.CreateNewPlayer(player.FirstName, player.LastName);

                var retVal = new CreatePlayerResponse(result)
                {
                    Status = ResponseStatus.Success
                };

                return CreatedAtAction(nameof(GetPlayer), new { id = retVal.Data.Id }, retVal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return this.BadRequest(new BaseResponse()
                {
                    Status = ResponseStatus.UnhandledException,
                    LogMessage = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserModel>> GetPlayer(int id)
        {
            _logger.LogInformation("GetPlayer method method has been called for next playerId {id}", id);
            return await _playerRepository.GetPlayer(id);
        }
    }
}
