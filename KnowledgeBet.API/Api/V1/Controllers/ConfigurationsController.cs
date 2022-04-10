using Domain.Interfaces.Repositories;
using KnowledgeBet.API.Api.V1.Models;
using KnowledgeBet.API.Api.V1.Models.Responses;
using Microsoft.AspNetCore.Mvc;

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

        public ConfigurationsController(ILogger<ConfigurationsController> logger, IConfigurationRepository configurationRepository)
        {
            _logger = logger;
            _configurationRepository = configurationRepository;
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
    }
}
