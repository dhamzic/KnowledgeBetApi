namespace KnowledgeBet.API.Api.V1.Models
{
    public class NewGameRequestModel
    {
        public DateTime Date { get; set; }
        public List<int> PlayersId { get; set; }
        public List<int> QuestionsId { get; set; }
    }
}
