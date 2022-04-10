using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class GetQuestionsFromSpecificCategoryResponse : BaseResponse
    {
        public List<QuestionModel> Data { get; set; } = new List<QuestionModel>();

    }
}
