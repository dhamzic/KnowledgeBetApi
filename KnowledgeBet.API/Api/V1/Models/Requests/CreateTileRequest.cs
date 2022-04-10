namespace KnowledgeBet.API.Api.V1.Models.Requests
{
    public class CreateTileRequest
    {
        public CreateTileRequest(string title, string route)
        {
            Title = title;
            Route = route;
        }

        public string Title { get; set; }
        public string Route { get; set; }

    }
}
