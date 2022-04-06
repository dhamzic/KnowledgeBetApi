using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class CreateGameResponse : BaseResponse
    {
        public CreateGameResponse(GameModel data)
        {
            Data = data;
        }

        public GameModel Data { get; set; }
    }
}
