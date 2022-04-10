using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class GetHomeComponentTilesResponse : BaseResponse
    {
        public List<HomeComponentTileModel> Data { get; set; } = new List<HomeComponentTileModel>();
    }
}
