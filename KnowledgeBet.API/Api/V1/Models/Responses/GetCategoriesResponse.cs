using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class GetCategoriesResponse : BaseResponse
    {
        public List<CategoryModel> Data { get; set; } = new List<CategoryModel>();

    }
}
