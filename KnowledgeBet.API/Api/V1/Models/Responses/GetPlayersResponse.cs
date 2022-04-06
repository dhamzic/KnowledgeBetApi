using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class GetPlayersResponse : BaseResponse
    {
        public List<UserModel> Data { get; set; } = new List<UserModel>();
    }
}
