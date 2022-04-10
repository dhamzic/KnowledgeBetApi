using Domain.Entities.Models;
using Domain.Interfaces.Repositories;
using KnowledgeBet.API.Api.V1.Models;
using KnowledgeBet.API.Api.V1.Models.Requests;
using KnowledgeBet.API.Api.V1.Models.Responses;
using KnowledgeBet.API.Api.V1.Models.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KnowledgeBet.API.Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/configurations")]
    [Produces("application/json")]
    public class ConfigurationsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IHubContext<AppHub> _hub;

        public ConfigurationsController(
            ILogger<ConfigurationsController> logger, 
            IConfigurationRepository configurationRepository, 
            IHubContext<AppHub> hub)
        {
            _logger = logger;
            _configurationRepository = configurationRepository;
            _hub = hub;
        }

        [HttpGet("home-component-tiles")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetHomeComponentTilesResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<GetHomeComponentTilesResponse>> GetHomeComponentTiles()
        {
            {
                try
                {
                    _logger.LogInformation("GetHomeComponentTiles method has been called");

                    var result = await _configurationRepository.GetHomeComponentTiles();

                    var retVal = new GetHomeComponentTilesResponse()
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

        [HttpPost("home-component-tiles")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateTileResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<CreateTileResponse>> CreateTile([FromBody] CreateTileRequest tile)
        {
            try
            {
                _logger.LogInformation("CreatePlayer method method has been called");

                var result = await _configurationRepository.CreateTile(tile.Title, tile.Route);

                var retVal = new CreateTileResponse(result)
                {
                    Status = ResponseStatus.Success
                };

                await _hub.Clients.All.SendAsync("transferTilesData", "New tile has been created.");

                return CreatedAtAction(nameof(GetTile), new { id = retVal.Data.Id }, retVal);
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
        public async Task<ActionResult<HomeComponentTileModel>> GetTile(int id)
        {
            _logger.LogInformation("GetTile method method has been called for next tileId {id}", id);
            return await _configurationRepository.GetTile(id);
        }
    }
}
