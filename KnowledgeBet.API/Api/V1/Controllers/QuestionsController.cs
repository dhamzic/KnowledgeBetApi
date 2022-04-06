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
    [Route("api/v{version:apiVersion}/questions")]
    [Produces("application/json")]
    public class QuestionsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(ILogger<QuestionsController> logger, IQuestionRepository questionRepository)
        {
            _logger = logger;
            _questionRepository = questionRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateQuestionResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<CreateQuestionResponse>> CreateQuestion([FromBody] CreateQuestionRequest createQuestionRequest)
        {
            try
            {
                _logger.LogInformation("CreateQuestion method method has been called");

                var newQuestionModel = new NewQuestionModel
                {
                    Text = createQuestionRequest.Text,
                    SubcategoryId = createQuestionRequest.SubcategoryId,
                    Options = createQuestionRequest.Options.Select(o => new QuestionOptionModel
                    {
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                };

                var result = await _questionRepository.CreateNewQuestion(newQuestionModel);

                var retVal = new CreateQuestionResponse(result)
                {
                    Status = ResponseStatus.Success
                };

                return CreatedAtAction(nameof(GetQuestion), new { id = retVal.Data.Id }, retVal);
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
        public async Task<ActionResult<QuestionModel>> GetQuestion(int id)
        {
            return await _questionRepository.GetQuestion(id);
        }

        [HttpDelete("{questionId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult> DeleteQuestion([FromRoute] int questionId)
        {
            try
            {
                _logger.LogInformation("DeleteQuestion method method has been called for next questionId {questionId}", questionId);

                await _questionRepository.DeleteQuestion(questionId);

                return this.NoContent();
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

        [HttpPut("{questionId}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult> DeactivateQuestion([FromRoute] int questionId)
        {
            try
            {
                _logger.LogInformation("DeactivateQuestion method method has been called for next questionId {questionId}", questionId);

                await _questionRepository.DeactivateQuestion(questionId);

                return this.NoContent();
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
