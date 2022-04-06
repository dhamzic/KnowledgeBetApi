namespace KnowledgeBet.API.Api.V1.Models.Requests
{
    public class CreateQuestionRequest
    {
        public CreateQuestionRequest(string text, int subcategoryId, List<QuestionOption> options)
        {
            Text = text;
            SubcategoryId = subcategoryId;
            Options = options;
        }

        public string Text { get; set; }
        public int SubcategoryId { get; set; }
        public List<QuestionOption> Options { get; set; }
    }
}
