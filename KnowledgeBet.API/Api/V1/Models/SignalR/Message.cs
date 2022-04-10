namespace KnowledgeBet.API.Api.V1.Models.SignalR
{
    public class Message
    {
        public Message(string user, string text)
        {
            this.User = user;
            this.Text = text;
        }

        public string User { get; set; }
        public string Text { get; set; }
    }
}
