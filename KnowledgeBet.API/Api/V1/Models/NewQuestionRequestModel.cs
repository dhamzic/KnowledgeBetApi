namespace KnowledgeBet.API.Api.V1.Models
{
    public class NewQuestionRequestModel
    {
        public string Text { get; set; }
        public int SubcategoryId { get; set; }
        public List<QuestionOption> Options { get; set; }
    }
}
