using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class CreateTileResponse : BaseResponse
    {
        public CreateTileResponse(HomeComponentTileModel data)
        {
            Data = data;
        }

        public HomeComponentTileModel Data { get; set; }
    }
}
