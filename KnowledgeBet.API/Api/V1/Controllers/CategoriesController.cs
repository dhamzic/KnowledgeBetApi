using Domain.Interfaces.Repositories;
using KnowledgeBet.API.Api.V1.Models;
using KnowledgeBet.API.Api.V1.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeBet.API.Api.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categories")]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCategoriesResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<GetCategoriesResponse>> GetCategories()
        {
            try
            {
                _logger.LogInformation("GetCategories method has been called");

                var result = await _categoryRepository.GetCategories();

                var retVal = new GetCategoriesResponse()
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
