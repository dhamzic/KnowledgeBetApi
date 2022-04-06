using Domain.Entities.Models;

namespace KnowledgeBet.API.Api.V1.Models.Responses
{
    public class CreatePlayerResponse : BaseResponse
    {
        public CreatePlayerResponse(UserModel data)
        {
            Data = data;
        }

        public UserModel Data { get; set; }
    }
}
