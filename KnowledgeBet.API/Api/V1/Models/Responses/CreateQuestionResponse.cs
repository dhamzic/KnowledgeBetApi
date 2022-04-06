using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class CreateQuestionResponse : BaseResponse
    {
        public CreateQuestionResponse(QuestionModel data)
        {
            Data = data;
        }

        public QuestionModel Data { get; set; }
    }
}
