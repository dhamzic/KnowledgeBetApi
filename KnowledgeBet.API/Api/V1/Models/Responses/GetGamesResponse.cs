using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class GetGamesResponse : BaseResponse
    {
        public List<PlayedGamesModel> Data { get; set; } = new List<PlayedGamesModel>();
    }
}
