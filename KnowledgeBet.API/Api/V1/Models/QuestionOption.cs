namespace KnowledgeBet.API.Api.V1.Models
{
    public class QuestionOption
    {
        public QuestionOption(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
        }

        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
