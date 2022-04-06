namespace KnowledgeBet.API.Api.V1.Models.Requests
{
    public class CreateGameRequest
    {
        public CreateGameRequest(DateTime date, List<int> playersId, List<int> questionsId, int winnerId)
        {
            Date = date;
            PlayersId = playersId;
            QuestionsId = questionsId;
            WinnerId = winnerId;
        }

        public DateTime Date { get; set; }
        public List<int> PlayersId { get; set; }
        public List<int> QuestionsId { get; set; }
        public int WinnerId { get; set; }
    }
}
